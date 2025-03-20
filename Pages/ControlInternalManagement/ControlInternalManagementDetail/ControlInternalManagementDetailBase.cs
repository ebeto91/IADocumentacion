using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalManagement.ControlInternalManagementDetail
{
    public class ControlInternalManagementDetailBase : ComponentBase
    {
        [Parameter]
        public string managementId { get; set; }

        public FullManagementDto ModelFirst { get; set; } = new FullManagementDto();
        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        public FullManagementDto PrincipalObject { get; set; } = new FullManagementDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        public List<SelectedItem> listWorkTaskPosition = new List<SelectedItem>();


        public List<string> listImagesSelected = new List<string>();

        public List<Catalog> listCatalogData = new List<Catalog>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IManagementService _managementService { get; set; }

        [Inject]
        public IDepartmentService _departmentService { get; set; }
        [Inject]
        public ICatalogAutomaticResponseService _catalogAutomaticResponseService { get; set; }
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }


        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Detail:Management";
        public Step? _stepper;
        public bool IsSecondPageReadyToLoad = false;

        [NotNull]
        public ImagePreviewer? ImagePreviewer { get; set; }

        public Task ShowImagePreviewer() => ImagePreviewer.Show();


        protected async override Task OnInitializedAsync()
        {

            _spinnerService.Show();
            ModelFirst ??= new();
            UserCreator ??= new();

            var isValid = await _validationRouteService.HasAccessRoute(validScopes);

            if (!isValid)
            {
                await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                _navigation.NavigateTo("/login");
                _spinnerService.Hide();
                return;
            }


            var response = await _managementService.GetManagementById(managementId);
            if (response != null && response.response != null && response.response.Success)
            {



                var fullManagementDto = response.definition;

                PrincipalObject = fullManagementDto;

                ModelFirst = fullManagementDto;

                if (ModelFirst.AttachedDocuments != null && ModelFirst.AttachedDocuments.Count > 0)
                {
                    foreach (var document in ModelFirst.AttachedDocuments)
                    {
                        listImagesSelected.Add(document.FilePath);
                    }
                }

                if (fullManagementDto.CreatedUserId.HasValue)
                {
                    var responseUser = await _userService.GetUserProfile(fullManagementDto.CreatedUserId.Value);
                    if (responseUser != null)
                    {
                        UserCreator = responseUser;
                    }
                }

                StateHasChanged();
                IsSecondPageReadyToLoad = true;
                _spinnerService.Hide();

            }
            else
            {

                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                goToList();
            }


            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT", "POSITION-WORK-TASK"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            ////
            var listStatus = listCatalogData.Where(x => x.Collection == "STATUS-MANAGEMENT");

            foreach (var item in listStatus)
            {
                listSelectStatus.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectStatus.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////


            ////
            var listPriority = listCatalogData.Where(x => x.Collection == "PRIORITY").OrderBy(x => x.Ref2);

            foreach (var item in listPriority)
            {
                listSelectPriority.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectPriority.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////

            ////
            var listSubtype = listCatalogData.Where(x => x.Collection == "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT");
            foreach (var item in listSubtype)
            {
                listSelectType.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectType.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////
            ///

            ////
            var listPosition = listCatalogData.Where(x => x.Collection == "POSITION-WORK-TASK");
            foreach (var item in listPosition)
            {
                listWorkTaskPosition.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listWorkTaskPosition.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////
            _spinnerService.Hide();
        }



        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("/gestionCiudadana/solicitudes");
        }
        #endregion

    }
}
