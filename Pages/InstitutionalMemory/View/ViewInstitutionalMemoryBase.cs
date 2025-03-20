using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.Edit.Details;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.Edit.Tabs.Historial;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.View
{
    public class ViewInstitutionalMemoryBase:ComponentBase
    {

        [Parameter]
        public int? id { get; set; } = 0;

        [Parameter]
        public string memoryId { get; set; }


        List<DistrictNeighborhoodsDefinition> listDistrictNeighborhoodsDefinition = new List<DistrictNeighborhoodsDefinition>();

        public List<AssociationResponseGroupByDistrict> associationResponseGroupByDistricts = new List<AssociationResponseGroupByDistrict>();


        [Inject]
        public IUserService _userService { get; set; }

        [Inject]
        public IDistrictService _districtService { get; set; }

        [Inject]
        public IWorkTaskService _workTaskService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        [Inject]
        public ICatalogAutomaticResponseService _catalogResponseService { get; set; }
        private string validScopes { get; } = "Edit:User";

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }


        public WorkTaskInputEditTrackableDto workTaskResponseDetail = new WorkTaskInputEditTrackableDto();


        public WorkTaskResponseDetail workTaskResponseDetailConsult = new WorkTaskResponseDetail();


        public List<Catalog> listCatalogData = new List<Catalog>();

        List<CatalogAutomaticResponseDto> listAutomaticResponse = new List<CatalogAutomaticResponseDto>();


        [Inject]
        public IDepartmentService _departmentService { get; set; }


        [Inject]
        public NavigationManager _navigation { get; set; }

        [NotNull]
        public Tab? TabSetTemplate { get; set; }

        [NotNull]
        public bool IsActiveDetail { get; set; } = true;

        [Inject]
        public IAssociationService _associationService { get; set; }

        [NotNull]
        public bool IsActiveHistory { get; set; } = true;

        public IEnumerable<Catalog> _catalogList { get; set; }
        [NotNull]
        public EditInstitutionalMemoryDetails editWorkTasksDetails { get; set; }

        public List<DepartmentResponse> departmentListData = new List<DepartmentResponse>();

        //public List<CatalogAutomaticResponseDto> listCatalogAutomaticResponse = new List<CatalogAutomaticResponseDto>();

        #region historial
        [NotNull]
        public EditWorkTasksHistorial editWorkTasksHistorial { get; set; }
        #endregion

        #region tabs methods
        public static string? GetClassString(TabItem tabItem) => CssBuilder.Default("tabs-item").AddClass("active", tabItem.IsActive).Build();

        public async Task OnClickTabItem(TabItem tabItem)
        {
            TabSetTemplate.ActiveTab(tabItem);


            var childText = tabItem.Text;
            switch (childText)
            {
                case TABSDATA.DETAIL:
                    IsActiveDetail = true;
                    //IsActiveAttachments = false;
                    IsActiveHistory = false;
                    break;

                case TABSDATA.ATTACHMENTS:
                    //IsActiveAttachments = true;
                    IsActiveDetail = false;
                    IsActiveHistory = false;
                    break;

                case TABSDATA.HISTORY:
                    IsActiveHistory = true;
                    // IsActiveAttachments = false;
                    IsActiveDetail = false;

                    await editWorkTasksHistorial.UpdateData(workTaskResponseDetailConsult, listCatalogData);
                    break;


            }

            StateHasChanged();
        }
        #endregion

        public async Task HandleValidSubmit()
        {

        }


        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("javascript:history.back()");
          /*  if (id.HasValue && id == 0)
            {
                _navigation.NavigateTo("/memoriaInstitucional");
            }
            else
            {
                _navigation.NavigateTo("/miperfil");
            }*/
        }
        #endregion



        protected async override Task OnInitializedAsync()
        {

            _spinnerService.Show();

            var response = await _workTaskService.GetWorkTaskById(memoryId);
            if (response != null && response.response != null && response.response.Success)
            {




                CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
                {
                    Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION", "POSITION-WORK-TASK", "TYPE-PROCCESS", "TYPE-WORKTASK", "DISTRICT", "NEIGHBORHOOD", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT"]
                };

                var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
                listCatalogData = listAllDataCatalog;

                var getDepartments = await _departmentService.GetAllDepartments();
                if (getDepartments != null && getDepartments.response != null && getDepartments.response.Success)
                {

                    departmentListData = getDepartments.definition.items;
                }
                _spinnerService.Hide();
                StateHasChanged();
                var detail = response.definition;

                workTaskResponseDetailConsult = detail;

                workTaskResponseDetail = detail.ToJson().FromJson<WorkTaskInputEditTrackableDto>();

                if (workTaskResponseDetail.AssociationRelatedMemory != null)
                {
                    workTaskResponseDetail.AssociationMemory = workTaskResponseDetail.AssociationRelatedMemory.Id.ToString();
                }

                var inputToSearch = new CatalogResponseInputListMultipleDto()
                {
                    Codes = ["DONE", "DONTAPPLY", "FORTHCOMINGBUDGET", "NEW"],
                    Enabled = true,
                };

                var listResponsesAutomatic = await _catalogResponseService.GetCatalogResponseForListByCodesList(inputToSearch);

                if (listResponsesAutomatic.response != null && listResponsesAutomatic.response.Success)
                {
                    listAutomaticResponse = listResponsesAutomatic.definition;
                }

                //IsActiveDetail = true;

                listDistrictNeighborhoodsDefinition = await _districtService.GetDistricts();


                var data = await _associationService.GetAssociationGroupByDistrict();
                associationResponseGroupByDistricts = data.definition;

                await editWorkTasksDetails.UpdateData(listCatalogData,
                    workTaskResponseDetailConsult.ListAssignedUsers,
                    workTaskResponseDetailConsult.ListUsersSelectable,
                    memoryId, workTaskResponseDetailConsult, listAutomaticResponse, listDistrictNeighborhoodsDefinition, associationResponseGroupByDistricts,true,null,false);
            }
            else
            {

                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                goToList();
            }


        }

        protected async Task ActionComponentFather()
        {
            //_spinnerService.Show();
            //var response = await _workTaskService.GetWorkTaskById(workTaskId);
            //if (response != null && response.response != null && response.response.Success)
            //{
            //    var getDepartments = await _departmentService.GetAllDepartments();
            //    if (getDepartments != null && getDepartments.response != null && getDepartments.response.Success)
            //    {

            //        departmentListData = getDepartments.definition.items;
            //    }
            //    StateHasChanged();
            //    var detail = response.definition;

            //    workTaskResponseDetailConsult = detail;

            //    workTaskResponseDetail = detail.ToJson().FromJson<WorkTaskInputEditTrackableDto>();

            //    await editWorkTasksDetails.UpdateData(listCatalogData,
            //       workTaskResponseDetailConsult.ListAssignedUsers,
            //       workTaskResponseDetailConsult.ListUsersSelectable,
            //       workTaskId, workTaskResponseDetailConsult);

            //    StateHasChanged();

            //    _spinnerService.Hide();
            //}

        }
    }
}
