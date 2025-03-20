using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalManagement.ControlInternalManagementList
{
    public class ControlInternalManagementListBase : ComponentBase
    {
        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;
        [Parameter]
        public ModelProfileManagementDefinition? listManagement { get; set; } = new ModelProfileManagementDefinition();

        public ManagementProfileInputFilterDto managementProfileInputFilterDto { get; set; } = new ManagementProfileInputFilterDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();



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
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT"]
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
            //


            await searchData();
            _spinnerService.Hide();
        }
        #region display
     
        public string getDisplayData(string valueCatalog)
        {
            if (!string.IsNullOrEmpty(valueCatalog))
            {
                var itemSelect = listCatalogData.FirstOrDefault(x => x.Code == valueCatalog);
                if (itemSelect != null)
                {
                    return itemSelect.DisplayLabel;
                }
            }
   
            return "Sin asignar";
        }

        public string getDateFormatDisplay(DateTime value)
        {
            return value.ToString("yyyy-MM-dd hh:mm tt");
        }
        #endregion


        #region search
        public async Task searchData()
        {
            _spinnerService.Show();
            actualPageIndex = 1;
            await SearchData();

            _spinnerService.Hide();
        }

        public async Task OnPageClick(int pageIndex)
        {

            _spinnerService.Show();
            actualPageIndex = pageIndex;

            await SearchData();

            _spinnerService.Hide();
        }

        private async Task SearchData()
        {
            managementProfileInputFilterDto.SkipCount = (actualPageIndex - 1) * 10;
            managementProfileInputFilterDto.MaxResultCount = 10;
            managementProfileInputFilterDto.PrincipalTypeApplication = "MANAGEMENT";


            var responseListManagement = await _managementService.GetAllManagementsFiltered(managementProfileInputFilterDto);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

            {
                // carga la data
                listManagement = responseListManagement.definition;
                var celling = Math.Ceiling((decimal)responseListManagement.definition.totalCount / 10);
                totalAcountPages = (int)celling;
                StateHasChanged();
            }
            else
            {
                //error
                var message = responseListManagement != null && responseListManagement.response != null ?
                    responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
            }
        }
        #endregion


        #region create new 

        public async Task createNewInternal()
        {

            _spinnerService.Show();
            _navigation.NavigateTo("/gestionCiudadana/solicitudes/internas/crear/solicitud");
            _spinnerService.Hide();
        }
        public async Task goToSeeDetailWorktask(Guid? workTaskId)
        {

            _spinnerService.Show();
            _navigation.NavigateTo($"/gestionCiudadana/tareas/detalle/{workTaskId}/0");
            _spinnerService.Hide();
        }
        #endregion
        #region download
        public async Task downloadReportExcel()
        {
            _spinnerService.Show();
            StateHasChanged();
            managementProfileInputFilterDto.SkipCount = (actualPageIndex - 1) * 10;
            managementProfileInputFilterDto.MaxResultCount = 10;
            managementProfileInputFilterDto.PrincipalTypeApplication = "MANAGEMENT";

            var responseListManagement = await _downloadService.GetAllManagementsFiltered(managementProfileInputFilterDto);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

            {
                // mostrar mensaje
                var message = responseListManagement != null && responseListManagement.response != null ?
                   responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Success("Acción", message, autoHide: true);
            }
            else
            {
                //error
                var message = responseListManagement != null && responseListManagement.response != null ?
                    responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
            }
            _spinnerService.Hide();
        }

        #endregion

        #region go to
        public void goToProcessEdit(ManagementProfileDto managementProfileDto)
        {
            _navigation.NavigateTo($"/gestionCiudadana/solicitudes/editar/{managementProfileDto.Id}");
        }

        public void goToProcessDetail(ManagementProfileDto managementProfileDto)
        {
            _navigation.NavigateTo($"/gestionCiudadana/solicitudes/detalle/{managementProfileDto.Id}");
        }
        #endregion
    }
}
