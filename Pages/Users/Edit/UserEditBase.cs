using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Data;
using System.Security.Claims;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Users.Edit
{
    public class UserEditBase : ComponentBase
    {

        [Parameter]
        public string userId { get; set; }

        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }
        [Inject]
        public IRoleService _roleService { get; set; }
        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Edit:User";

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        [SupplyParameterFromForm]
        public ManagementUserDto Model { get; set; }
        public ManagementUserDto userResponseManagementClone { get; set; }


        public List<SelectedItem> listIdentificationType { get; set; }
        public List<SelectedItem> listRoles { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }
        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        [Inject]
        Blazored.LocalStorage.ILocalStorageService localstorage { get; set; }
        public bool showForEditDropdown { get; set; }


        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();
            Model ??= new();


            var isValid = await _validationRouteService.HasAccessRoute(validScopes);

            if (!isValid)
            {
                await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                _navigation.NavigateTo("/login");
                _spinnerService.Hide();
                return;
            }

            var list = await _catalogService.GetCatalogByCollection("TYPE-IDENTIFICATION");
            List<SelectedItem> listSelect = new List<SelectedItem>();
            foreach (var item in list)
            {
                listSelect.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelect.Insert(0, (new SelectedItem { Text = "Seleccione el tipo de cedula", Value = "" }));
            listIdentificationType = listSelect;




            UserInputControlDto userInputControlDto = new UserInputControlDto()
            {
                Id = userId
            };

            var response = await _userService.GetUserWithRole(userInputControlDto);
            if (response != null && response.response != null && response.response.Success)
            {
                var userResponseManagement = response.definition.ToJson().FromJson<ManagementUserDto>();

                userResponseManagementClone = Utility.Clone(userResponseManagement);



                // Obtener el JWT desde localStorage
                var userTokenData = await _customAuthService.GetClaims();

                var roleAdminstrative = "";
                if (userTokenData != null)
                {
                    //var roleClaimType = userTokenData.User.RoleClaimType;
                    var roles = userTokenData.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                    var role = roles[0].Value;
                    roleAdminstrative = role;
                }


                var listRolesData = await _roleService.GetRoles();
                var roleName = "";
                if (!string.IsNullOrEmpty(roleAdminstrative))
                {
                    var roles = listRolesData.Where(x => x.FilterRole.Split(',').Any(r => r.Trim() == roleAdminstrative)).ToList();

                    var isRoleUserInList = roles.FirstOrDefault(x => x.Id == userResponseManagementClone.RoleId);
                    if (isRoleUserInList != null)
                    {
                        listRolesData = roles;
                        showForEditDropdown = true;
                    }
                    else
                    {
                        var foundNameRol = listRolesData.FirstOrDefault(x => x.Id == userResponseManagementClone.RoleId);

                        if (foundNameRol != null)
                        {
                            roleName = foundNameRol.Name;
                        }

                        showForEditDropdown = false;
                    }
                    StateHasChanged();

                }

                List<SelectedItem> listRolesSelect = new List<SelectedItem>();
                foreach (var item in listRolesData)
                {
                    listRolesSelect.Add(new SelectedItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                    });
                }
                listRolesSelect.Insert(0, (new SelectedItem { Text = "Seleccione el tipo de rol de usuario", Value = "" }));
                listRoles = listRolesSelect;



                Model = userResponseManagement;
                Model.RoleName = roleName;
                StateHasChanged();
                _spinnerService.Hide();

            }
            else
            {

                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                goToList();
            }


        }

        public async Task HandleValidSubmit()
        {
            var input = Model;
            // Do something with the form values
            // async/await
            input.Password = "cambiame";
            //input.Enabled = true;
            input.UserName = input.Name + " " + input.Lastname;
            //input.UserType = USERTYPE.INTERNAL;
            //input.IsEmailConfirmed = true;

            _spinnerService.Show();
            var response = await _userService.EditUser(input);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información editada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                goToList();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

        }

        public void HandleInvalidSubmit()
        {

            // Do something with the form values
        }


        public async void FormSubmitted()
        {
            this._spinnerService.Show();
            await _userService.CreateUser(Model);
            this._spinnerService.Hide();
            // Post data to the server, etc
        }

        public async Task OnItemChanged(SelectedItem item)
        {
            if (userResponseManagementClone.IdentificationTypeId != Model.IdentificationTypeId)
            {
                userResponseManagementClone.IdentificationTypeId = null;
                Model.Identification = "";
                StateHasChanged();
            }


        }

        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("/catalogo/usuarios");
        }
        #endregion

    }
}
