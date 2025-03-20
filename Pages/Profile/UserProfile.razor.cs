using BlazorSpinner;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using BootstrapBlazor.Components;
using Blazored.LocalStorage;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.JSInterop;
using System.Data;
using Microsoft.AspNetCore.Components.Authorization;
using DocumentFormat.OpenXml.Wordprocessing;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Menu;
using System.Security.Claims;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;


namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile
{
    public partial class UserProfile : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }



        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        Blazored.LocalStorage.ILocalStorageService localstorage { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        public string ProfilePicture { get; set; } = "https://www.shutterstock.com/image-vector/vector-flat-illustration-grayscale-avatar-600nw-2264922221.jpg";
        public bool NotificarPorCorreo { get; set; }
        public bool NotificarPorSms { get; set; }
        public bool SharedLocation { get; set; }

        public bool isUserAssociation { get; set; }
        //[CascadingParameter]
        //public Task<AuthenticationState>? authenticationState { get; set; }

        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        [SupplyParameterFromForm]
        public UserProfileResponse Model { get; set; } = new UserProfileResponse();
        public UserProfileResponse userResponseManagementClone { get; set; }
        private const long MaxFileSize = 512000; // 500 KB

        public IEnumerable<SelectedItem> itemsDistrito;
        public IEnumerable<SelectedItem> itemsBarrio;
        public IEnumerable<SelectedItem> itemsAsociaciones;

        private async Task OnProfilePictureChange(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file.Size > MaxFileSize)
            {
                // Mostrar mensaje de error
                await _toastService.Error("Ha ocurrido un error", $"El archivo excede el tamaño máximo permitido de {MaxFileSize / 1024} KB.", autoHide: true);
                return;
            }

            _spinnerService.Show();
            //Logica PARA ACTUALIZAR LA IMAGEN DEL PERFIL ACA
            ProfilePicture = null;
            Model.ImageProfile = null;
            StateHasChanged();
            Thread.Sleep(150);
            var image = "";
            var response = await _userService.UpdateImageUserProfile(new UpdateImageUserProfile { AttachedFile = file, UserId = Guid.Parse(userId) });
            if (response != null && response.Definition != null)
            {
                await Task.Delay(2000);
                image = response.Definition.FilePath;
                ProfilePicture = $"{image}?t={DateTime.Now.Ticks}"; // Añadir un parámetro único para evitar el caché
                Model.ImageProfile = ProfilePicture;
                StateHasChanged();
            }

            StateHasChanged();
            _spinnerService.Hide();
        }

        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();
            var userTokenData = await _customAuthService.GetClaims();

            if (userTokenData != null)
            {
                var uniqueNameClaim = userTokenData.Claims.FirstOrDefault(c => c.Type == "AsociationId");
                var AsociationId = uniqueNameClaim?.Value ?? "";

                if (!string.IsNullOrEmpty(AsociationId))
                {
                    isUserAssociation = true;
                }
                else
                {
                    isUserAssociation = false;
                }
            }


            var userIdItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "UserId");
            var response = await _userService.GetUserProfile(Guid.Parse(userIdItem.Value));

            if (response != null && response.Name != null)
            {

                userId = userIdItem.Value;
                userResponseManagementClone = Utility.Clone(response);

                if (isUserAssociation)
                {
                    List<SelectedItem> listSelectDistrict = new List<SelectedItem>();
                    List<SelectedItem> listSelectBarrio = new List<SelectedItem>();
                    List<SelectedItem> listSelectAso = new List<SelectedItem>();

                    listSelectDistrict.Insert(0, (new SelectedItem { Text = !string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.DistrictLabel) ? userResponseManagementClone.AssociationRelated.DistrictLabel : "Sin Distrito", Value = "1" }));
                    listSelectBarrio.Insert(0, (new SelectedItem { Text = !string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.NeighbordLabel) ? userResponseManagementClone.AssociationRelated.NeighbordLabel : "Sin Barrio", Value = "1" }));
                    listSelectAso.Insert(0, (new SelectedItem { Text = !string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.Name) ? userResponseManagementClone.AssociationRelated.Name : "Sin Asociación", Value = "1" }));

                    itemsDistrito = listSelectDistrict;
                    itemsBarrio = listSelectBarrio;
                    itemsAsociaciones = listSelectAso;
                }
                Model = userResponseManagementClone;

                NotificarPorCorreo = userResponseManagementClone.UserMethodNotifications.Any(n => n.Value == "EMAIL" && n.Selected);

                NotificarPorSms = userResponseManagementClone.UserMethodNotifications.Any(n => n.Value == "SMS" && n.Selected);

                SharedLocation = userResponseManagementClone.SharedLocation ? true : false;

                StateHasChanged();
            }
            _spinnerService.Hide();
        }

        public void HandleOnChange(ChangeEventArgs e)
        {
            SharedLocation = (bool)e.Value;
        }

        private string ParseJwtForClaim(string jwt, string claimType)
        {
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwt);

                // Buscar la claim específica
                var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);
                return claim?.Value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void ActualizarInformacion()
        {
            // Lógica para actualizar la información del usuario
        }

        private async Task HandleValidSubmitAsync()
        {

            _spinnerService.Show();

            if (NotificarPorSms)
            {
                var existSms = Model.UserMethodNotifications.FirstOrDefault(x => x.Value == "SMS");
                if (existSms == null)
                {
                    Model.UserMethodNotifications.Add(new UserMethodNotificationDto()
                    {
                        Name = "Mensaje SMS",
                        Description = "Aplicación de notificación por SMS",
                        Selected = true,
                        Value = "SMS"
                    });
                }

            }

            foreach (var item in Model.UserMethodNotifications)
            {
                if (item.Value == "EMAIL")
                {
                    item.Selected = NotificarPorCorreo;
                }
                else if (item.Value == "SMS")
                {
                    item.Selected = NotificarPorSms;
                }
            }




            Model.SharedLocation = SharedLocation;
            var response = await _userService.UpdateUserProfile(Model);
            if (response != null && response.Response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.Response != null ? response.Response.Message : "Información editada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.Response != null ? response.Response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
        }

        private void HandleInvalidSubmit()
        {
        }
    }
}
