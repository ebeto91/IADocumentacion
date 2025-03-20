using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalComplaint.ControlInternalComplaintDetail
{
    public class ControlInternalComplaintDetailBase : ComponentBase
    {

        [Parameter]
        public string managementId { get; set; }

        public FullComplaitDto ModelFirst { get; set; } = new FullComplaitDto();

        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        public FullComplaitDto PrincipalObject { get; set; } = new FullComplaitDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        public List<SelectedItem> listWorkTaskPosition = new List<SelectedItem>();

        public List<SelectedItem> listDepartmentsSelect = new List<SelectedItem>();


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
        public IDistrictService _districtService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Detail:Complaint";

        public bool IsSecondPageReadyToLoad = false;

        public List<DepartmentResponse> departmentListData = new List<DepartmentResponse>();
        public List<UserDepartmentDto> userForListWithDepartment = new List<UserDepartmentDto>();
        public List<CatalogAutomaticResponseDto> listCatalogAutomaticResponse = new List<CatalogAutomaticResponseDto>();
        public List<SelectedItem> listDistricts = new List<SelectedItem>();




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

                var listDistrictsItems = await _districtService.GetDistricts();

                foreach (var item in listDistrictsItems)
                {
                    listDistricts.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });


                }
                listDistricts.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));


                var fullManagementDto = response.definition.ToJson().FromJson<FullComplaitDto>();

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


                var getDepartments = await _departmentService.GetAllDepartments();
                if (getDepartments != null && getDepartments.response != null && getDepartments.response.Success)
                {

                    departmentListData = getDepartments.definition.items;


                    listDepartmentsSelect.Add(new SelectedItem()
                    {
                        Text = "Seleccionar para filtrar",
                        Value = ""
                    });

                    foreach (var item in departmentListData)
                    {
                        listDepartmentsSelect.Add(new SelectedItem()
                        {
                            Text = item.Name,
                            Value = item.Id.ToString()
                        });
                    }
                }



                var getAllUserDepartments = await _departmentService.GetDeparmentsWithUsers();
                if (getAllUserDepartments != null && getAllUserDepartments.response != null && getAllUserDepartments.response.Success)
                {

                    userForListWithDepartment = getAllUserDepartments.definition;
                }


                var objectResponseCatalog = await _catalogAutomaticResponseService.GetCatalogResponseForList(new CatalogResponseInputListDto()
                {
                    Enabled = true
                });

                if (objectResponseCatalog != null && objectResponseCatalog.response != null && objectResponseCatalog.response.Success)
                {

                    listCatalogAutomaticResponse = objectResponseCatalog.definition;
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
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-COMPLAINT", "POSITION-WORK-TASK",]
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
            var listSubtype = listCatalogData.Where(x => x.Collection == "SUBPRINCIPALTYPE-APPLICATION-COMPLAINT");
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
            _navigation.NavigateTo("/gestionCiudadana/denuncias");
        }
        #endregion


        #region table

        [NotNull]
        public Table<ManagementAttachedDocumentDto>? Table { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<ManagementAttachedDocumentDto>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<ManagementAttachedDocumentDto> items = new List<ManagementAttachedDocumentDto>();
            if (ModelFirst.AttachedDocuments != null)
            {
                items = ModelFirst.AttachedDocuments;

                return new QueryData<ManagementAttachedDocumentDto>()
                {
                    Items = items,
                    TotalCount = items.Count(),
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
            else
            {
                return new QueryData<ManagementAttachedDocumentDto>()
                {
                    Items = items,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };

            }





        }

        #endregion

    }
}
