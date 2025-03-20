using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using DocumentFormat.OpenXml.Spreadsheet;
using LeafletForBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using NPOI.SS.Formula.Functions;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Cords;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Security.Claims;
using static LeafletForBlazor.RealTimeMap;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CitizenManagement
{
    public class CitizenManagmentBase : ComponentBase
    {
        public UserCredenditialsDto userCredenditialsDto = new UserCredenditialsDto();
        public List<EvidenciaDto> imageDataUrl = new List<EvidenciaDto>();
        public const long MaxFileSize = 10240000L; // 10mb

        [Inject]
        public ToastService _toastService { get; set; }

        public bool SharedLocation { get; set; }
        public bool SharedUbicationPerson { get; set; } = false;
        public bool ShowMap { get; set; } = true;

        [NotNull]
        public Modal? ModalMap { get; set; }

        public RealTimeMap realTimeMap = new RealTimeMap();

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }

        private Coordinates? ModelPosition { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IUserService _userService { get; set; }
        public IEnumerable<SelectedItem> items;

        public IEnumerable<GetUserForDropDownList> users;

        //private const long MaxFileSize = 10240000L;
        public IEnumerable<SelectedItem> itemsDistrito;
        public IEnumerable<SelectedItem> itemsBarrio;
        public IEnumerable<SelectedItem> itemsAsociaciones;

        public bool isUserAssociation { get; set; }

        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        [Inject]
        public ICitizenManagment _citizenManagmentService { get; set; }

        public CreateManagementInputDto createManagementInputDto = new CreateManagementInputDto();

        public static string OnGetDisplayText(GetUserForDropDownList foo) => foo.Identification ?? "".Trim();

        public GetUserForDropDownList Model = new GetUserForDropDownList();

        public CitizenManagmentDto citizenManagmentDto = new CitizenManagmentDto();
        public UserProfileResponse userResponseManagementClone { get; set; }
        public int characterCount = 0;


        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        public DotNetObjectReference<CitizenManagmentBase> self;

        string latitude = BASELOCATION.latitude;
        string longitude = BASELOCATION.longitude;
        public List<string> StaticItems = new List<string>();


        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();

            var userTokenData = await _customAuthService.GetClaims();
            var userId = "";
            if (userTokenData != null)
            {
                var userIdItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "UserId");

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


                var response = await _userService.GetUserProfile(Guid.Parse(userIdItem.Value));
                if (response != null && response.Name != null)
                {


                    userResponseManagementClone = Utility.Clone(response);

                    createManagementInputDto.ManagementName = userResponseManagementClone.Name + " " + userResponseManagementClone.Lastname;
                    createManagementInputDto.ManagementEmail = userResponseManagementClone.EmailAddress;
                    createManagementInputDto.ManagementPhone = userResponseManagementClone.PhoneNumber;
                    citizenManagmentDto.Identification = userResponseManagementClone.Identification;


                    if (isUserAssociation)
                    {
                        List<SelectedItem> listSelectDistrict = new List<SelectedItem>();
                        List<SelectedItem> listSelectBarrio = new List<SelectedItem>();
                        List<SelectedItem> listSelectAso = new List<SelectedItem>();

                        if (!string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.DistrictCode))
                        {
                            listSelectDistrict.Insert(0, (new SelectedItem { Text = userResponseManagementClone.AssociationRelated.DistrictLabel, Value = userResponseManagementClone.AssociationRelated.DistrictCode }));
                        }
                        else
                        {
                            listSelectDistrict.Insert(0, (new SelectedItem { Text = "Sin distrito", Value = "" }));
                        }

                        if (!string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.NeighbordCode))
                        {
                            listSelectBarrio.Insert(0, (new SelectedItem { Text = userResponseManagementClone.AssociationRelated.NeighbordLabel, Value = userResponseManagementClone.AssociationRelated.NeighbordCode }));
                        }
                        else
                        {
                            listSelectBarrio.Insert(0, (new SelectedItem { Text = "Sin Barrio", Value = "" }));
                        }

                        //listSelectBarrio.Insert(0, (new SelectedItem { Text = !string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.NeighbordLabel) ? userResponseManagementClone.AssociationRelated.NeighbordLabel : "Sin Barrio", Value = userResponseManagementClone.AssociationRelated.NeighbordCode }));
                        listSelectAso.Insert(0, (new SelectedItem { Text = !string.IsNullOrEmpty(userResponseManagementClone.AssociationRelated.Name) ? userResponseManagementClone.AssociationRelated.Name : "Sin Asociación", Value = "1" }));

                        createManagementInputDto.ManagementName = userResponseManagementClone.AssociationRelated.Name;

                        itemsDistrito = listSelectDistrict;
                        itemsBarrio = listSelectBarrio;
                        itemsAsociaciones = listSelectAso;

                        createManagementInputDto.District = userResponseManagementClone.AssociationRelated.DistrictCode ?? "";
                        createManagementInputDto.Neighborhood = userResponseManagementClone.AssociationRelated.NeighbordCode ?? "";
                        createManagementInputDto.AssociationRelatedMemoryId = userResponseManagementClone.AssociationRelated.Id;

                    }


                    StateHasChanged();
                }
            }







            _spinnerService.Hide();
            //return base.OnInitializedAsync();
        }

        public void HandleOnChange(ChangeEventArgs e)
        {
            SharedLocation = (bool)e.Value;
        }

        public async Task HandleSubmit()
        {



        }

        public async Task SearchCitizen()
        {
            _spinnerService.Show();
            if (!string.IsNullOrEmpty(Model.Identification))
            {

                UserIdentificationInputDto input = new UserIdentificationInputDto
                {
                    Identification = Model.Identification
                };
                var list = await _userService.GetAllUserResponse(input);

                users = list.definition;

                var user = users.FirstOrDefault(x => x.Identification == Model.Identification);
                if (user != null)
                {
                    createManagementInputDto.ExternalManagementName = user.Name + " " + user.Lastname;
                    createManagementInputDto.ExternalManagementEmail = user.EmailAddress;
                    createManagementInputDto.ExternalManagementPhone = user.PhoneNumber ?? "";

                }
                else
                {
                    createManagementInputDto.ExternalManagementName = "";
                    createManagementInputDto.ExternalManagementEmail = "";
                    createManagementInputDto.ExternalManagementPhone = "";
                    await _toastService.Information("Añadir participante", "No se encontro el participante, puede digitar el nombre, correo y telefono", autoHide: true);
                }
            }
            else
            {
                await _toastService.Information("Añadir participante", "Se debe de un indicar una identificación", autoHide: true);

            }

            StateHasChanged();
            _spinnerService.Hide();
        }

        public Task OnItemChanged(GetUserForDropDownList item)
        {
            citizenManagmentDto.NameP = item.Name + " " + item.Lastname;
            citizenManagmentDto.EmailP = item.EmailAddress;
            citizenManagmentDto.PhoneNumberP = item.PhoneNumber ?? "";
            StateHasChanged();
            return Task.CompletedTask;


        }

        public void checkName(KeyboardEventArgs e)
        {
            if (string.IsNullOrEmpty(citizenManagmentDto.NameP))
            {
                Model = null;
            }

        }

        public void ClearIfNoMatch(ChangeEventArgs e)
        {
            var inputValue = e.Value?.ToString() ?? string.Empty;

            // Check if the input matches any user in the list
            var match = users.FirstOrDefault(user =>
                $"{user.Name} {user.Lastname}".Contains(inputValue, StringComparison.OrdinalIgnoreCase));

            // Clear the model if no match is found
            if (match == null)
            {
                Model = null;
            }
        }


        public async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            var format = "image/png";
            foreach (var image in imageFiles)
            {
                var totalDeleteFalse = imageDataUrl != null && imageDataUrl.Count>0 ? imageDataUrl.Where(x => x.Delete == false).Count():0;
                if(imageDataUrl!=null && totalDeleteFalse == 4) {
                    await _toastService.Information($"Cantidad de Evidencias", $"Se permiten 4 Imágenes.", autoHide: true);
                    return;
                }

                if (image.Size > MaxFileSize)
                {
                    // Mostrar mensaje de error
                    await _toastService.Error($"Ha ocurrido un error con la imagen {image.Name}", $"El archivo excede el tamaño máximo permitido de 10 Mb.", autoHide: true);

                }
                else
                {
                    var resizeImageFile = await image.RequestImageFileAsync(format, 250, 250);
                    var buffer = new byte[resizeImageFile.Size];
                    await resizeImageFile.OpenReadStream(maxAllowedSize: MaxFileSize).ReadAsync(buffer);

                    var imageDataUrlLink = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                    var fileStreamContent = new StreamContent(image.OpenReadStream(maxAllowedSize: MaxFileSize));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);

                    EvidenciaDto evidenciaDto = new EvidenciaDto()
                    {
                        imageUrl = imageDataUrlLink,
                        Identificador = Guid.NewGuid().ToString(),
                        Delete = false,
                        FileImage = image,
                        streamContentImage = fileStreamContent

                };
                    imageDataUrl.Add(evidenciaDto);
                }
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/CitizenManagement/CitizenManagment.razor.js");

                self = DotNetObjectReference.Create(this);

                await module.InvokeVoidAsync("Init", latitude, longitude, self);


                createManagementInputDto.LatitudeS = latitude.ToString();
                createManagementInputDto.LongitudeS = longitude.ToString();

                createManagementInputDto.Latitude = latitude;
                createManagementInputDto.Longitude = longitude;
                StateHasChanged();
            }

        }

        [JSInvokable]
        public void returnOnClickCoordinates(string latitude, string longitude)
        {
            createManagementInputDto.LatitudeS = latitude.ToString();
            createManagementInputDto.LongitudeS = longitude.ToString();

            createManagementInputDto.Latitude = latitude;
            createManagementInputDto.Longitude = longitude;

            StateHasChanged();
        }

        public async void HandleDeleteImage(string id)
        {

            if (imageDataUrl.Count > 0)
            {



                //  await module.InvokeVoidAsync("DeleteImage", id);
                var image = imageDataUrl.FirstOrDefault(x => x.Identificador == id);
                if (image != null)
                {
                    //imageDataUrl.Remove(image);
                    image.Delete = true;
                }
            }
            StateHasChanged();
        }

        public async Task OnShownCallbackAsync()
        {
            await Task.Delay(100);
            StateHasChanged();
            // return Task.CompletedTask;
        }

        public async void HandleCreateCitizenManagment()
        {
            _spinnerService.Show();
           /* var images = imageDataUrl
          .Where(e => e.FileImage != null && e.Delete==false)
          .Select(e => e.FileImage)
          .ToList();*/

            List<EvidenciaDto> listImg = new List<EvidenciaDto>();
            foreach (var item in imageDataUrl)
            {
                if (!item.Delete)
                {
                    listImg.Add(item);
                }
            }

            createManagementInputDto.streamContent = listImg;
            createManagementInputDto.IsAnonymous = SharedLocation;
            createManagementInputDto.PrincipalTypeApplication = PRINCIPALTYPE.MANAGEMENT;

            if (createManagementInputDto.Latitude == BASELOCATION.latitude && createManagementInputDto.Longitude == BASELOCATION.longitude)
            {
                createManagementInputDto.Latitude = null;
                createManagementInputDto.Longitude = null;
            }


            var result = await _citizenManagmentService.CreateCitizenManagment(createManagementInputDto,true);
            if (result != null && result.response.Success)
            {
                _spinnerService.Hide();
                await ToastService.Success("Solicitud", result.response.Message, autoHide: true);
                Navigation.NavigateTo("/miperfil");
            }
            else
            {
                _spinnerService.Hide();
                await ToastService.Error("Solicitud", "Ha ocurrido un problema, por favor intentarlo de nuevo.", autoHide: true);
            }
        }


        public async Task AskForActualUbication()
        {
            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Compartir Ubicación",
                Text = "¿Se encuentra en la misma ubicación del reporte del incidente?",
                Icon = SweetAlertIcon.Info,
                AllowEscapeKey = false,
                ShowCancelButton = true,
                CancelButtonColor = "#D9001B",
                ConfirmButtonText = "Compartir ubicación",
                ConfirmButtonColor = "#0755A3",
                CancelButtonText = "No"
            });
       
            if (result != null && result.IsConfirmed)
            {
                _spinnerService.Show();
                string positionServiceData = await _jsRuntime.InvokeAsync<string>("getCurrentPosition");
                if (positionServiceData != null)
                {

                    ModelPosition = positionServiceData.FromJson<Coordinates>();
                    if (ModelPosition.coords.accuracy < 600)
                    {
                        var format = String.Format("{0:0.000000}", ModelPosition.coords.latitude);
                        //latitude = ModelPosition.LastLat.ToString("0.000000").Replace(",", ".");
                        latitude = format.Replace(",", ".");
                        //longitude = ModelPosition.LastLong.ToString("0.000000").Replace(",", ".");
                        var formatLongitude = String.Format("{0:0.000000}", ModelPosition.coords.longitude);
                        longitude = formatLongitude.Replace(",", ".");


                        await module.InvokeVoidAsync("AddedMark", latitude, longitude);


                        createManagementInputDto.LatitudeS = latitude.ToString();
                        createManagementInputDto.LongitudeS = longitude.ToString();

                        createManagementInputDto.Latitude = latitude;
                        createManagementInputDto.Longitude = longitude;
                        SharedUbicationPerson = true;
                        StateHasChanged();
                     

                    }
                    else
                    {
                        await ToastService.Information("Obtener ubicación", "No se logro obtener con presicion su ubicación actual, por favor utilizar el mapa.", autoHide: true);
                    }
                }

                _spinnerService.Hide();

            }



            StateHasChanged();
        }

        public void onClickMap(RealTimeMap.ClicksMapArgs value)
        {
            //where value.sender is RealTimeMap control reference

            //createManagementInputDto.LatitudeS = value.location.latitude.ToString();
            //createManagementInputDto.LongitudeS = value.location.longitude.ToString();

            //PointIcon icon = new PointIcon()
            //{
            //    iconUrl = "https://cdn4.iconfinder.com/data/icons/small-n-flat/24/map-marker-512.png",
            //    iconSize = new[] { 50, 50 },
            //    iconAnchor = new int[] { 50, 50 }
            //};
            //realTimeMap.movePoint([value.location.latitude, value.location.longitude], icon);
        }

        public void UpdateCharacterCount(ChangeEventArgs e)
        {
            var input = e.Value?.ToString() ?? string.Empty;
            characterCount = input.Length;
        }
        //public string ShowMapText()
        //{
        //    if (ShowMap)
        //    {
        //        ShowMap = false; 
        //        StateHasChanged();
        //        return "Ocultar Mapa";
        //    }
        //    else
        //    {
        //        ShowMap = true;
        //        StateHasChanged();
        //        return "Mostrar Mapa";
        //    }
        //}

    }


}
