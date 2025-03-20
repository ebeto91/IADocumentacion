using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.Edit.Details;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.List;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory
{
    public class InstitutionalMemoryBase : ComponentBase
    {


        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;
        public ManagementProfileInputFilterDto managementProfileInputFilterDto { get; set; } = new ManagementProfileInputFilterDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        public IAssociationService _associationService { get; set; }

        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        public List<SelectedItem> listSelectDistrict = new List<SelectedItem>();

        public List<SelectedItem> listSelectAssociation = new List<SelectedItem>();

        public List<AssociationResponseGroupByDistrict> associationResponseGroupByDistricts = new List<AssociationResponseGroupByDistrict>();

        [NotNull]
        public ListInstitutionalMemory listInstitutionalMemory { get; set; }


        public List<Catalog> listCatalogData = new List<Catalog>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IManagementService _managementService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IDownloadExcelService _downloadService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();




            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION", "DISTRICT", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            //
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
            //


            //
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
            //

            //
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
            

            var listDistrict= listCatalogData.Where(x => x.Collection == "DISTRICT");
            foreach (var item in listDistrict)
            {
                listSelectDistrict.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectDistrict.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));

            var data = await _associationService.GetAssociationGroupByDistrict();
            if(data != null && data.response.Success)
            {
                associationResponseGroupByDistricts = data.definition;
            }

        // await searchData();
        _spinnerService.Hide();

        }

        public  Task OnItemChanged(SelectedItem item)
        {
            var valuePick=item.Value;
            if (valuePick != null)
            {
                var list = associationResponseGroupByDistricts.FirstOrDefault(x => x.CodeDistrict == item.Value);

                if (list != null && list.ListAssociations.Count > 0)
                {
                    if (listSelectAssociation.Count > 0)
                    {
                        listSelectAssociation.Clear();
                    }

                    foreach (var itemDistrictssociation in list.ListAssociations)
                    {
                        listSelectAssociation.Add(new SelectedItem()
                        {
                            Text = itemDistrictssociation.Name,
                            Value = itemDistrictssociation.Id.ToString(),
                        });
                    }

                    listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                }
                else
                {

                    listSelectAssociation.Clear();

                    listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                }
            }
            else
            {
                listSelectAssociation.Clear();

                listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            }
            StateHasChanged();
            return Task.CompletedTask;

        }

        public void CreateInstitutionalMemory()
        {
            _navigation.NavigateTo("crearMemoriaInstitucional");
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

        public async void Search()
        {
            await listInstitutionalMemory.UpdateData(managementProfileInputFilterDto,true);
        }

        public async void DownloadMemory()
        {
            await listInstitutionalMemory.downloadReportExcel();
        }
    }
}
