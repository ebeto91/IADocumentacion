using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.List
{
    public class ListInstitutionalMemoryBase:ComponentBase
    {
        [Parameter]
        public bool autoLoad { get; set; } = false;

        [Parameter]
        public bool isFromPerfilNavigation { get; set; } = false;

        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;

        public List<Catalog> listCatalogData = new List<Catalog>();
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public IDownloadExcelService _downloadService { get; set; }

        [Parameter]
        public bool isFromPerfil { get; set; }

        public bool justComments { get; set; } = true;

        [Parameter]
        public string? UserAssignedId { get; set; } //para usuario interno

        [Parameter]
        public string? AssociationRelatedMemoryId { get; set; } //para usuario asociación

        public ModelProfileManagementDefinition? listManagement { get; set; } = new ModelProfileManagementDefinition();

        public ManagementProfileInputFilterDto managementProfileInputFilterDto { get; set; } = new ManagementProfileInputFilterDto();

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IManagementService _managementService { get; set; }
    
        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;


            if (autoLoad)
            {
                await searchData();
            }
          
        }

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
            managementProfileInputFilterDto.PrincipalTypeApplication = PRINCIPALTYPE.INSTITUTIONALMEMORY;


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


        public string getDateFormatDisplay(DateTime value)
        {
            return value.ToString("yyyy-MM-dd hh:mm tt");
        }

        public void goToProcessEdit(string id)
        {
            if (!isFromPerfilNavigation)
            {
                _navigation.NavigateTo($"/memoria/editar/{id}/0");
            }
            else
            {
                _navigation.NavigateTo($"/memoria/editar/{id}/1");
            }
        }

        public void goToProcessDetail(ManagementProfileDto managementProfileDto)
        {
            if (!isFromPerfilNavigation)
            {
                _navigation.NavigateTo($"/memoria/ver/{managementProfileDto.WorkTaskAssignedBefore}/0");
            }
            else
            {
                _navigation.NavigateTo($"/memoria/ver/{managementProfileDto.WorkTaskAssignedBefore}/1");
            }
        }

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

      

        public async Task goToSeeDetailWorktask(Guid? workTaskId)
        {

            _spinnerService.Show();
            _navigation.NavigateTo($"/memoria/ver/{workTaskId}");
            _spinnerService.Hide();
        }

        public async Task goToEditWorktask(Guid? workTaskId)
        {

            _spinnerService.Show();
            _navigation.NavigateTo($"/gestionCiudadana/tareas/detalle/{workTaskId}");
            _spinnerService.Hide();
        }
        public async Task UpdateData(ManagementProfileInputFilterDto input,bool CollaborateEdit=true)
        {

            justComments = CollaborateEdit;
            _spinnerService.Show();
            actualPageIndex = 1;
            input.SkipCount = (actualPageIndex - 1) * 10;
            input.MaxResultCount = 10;
            input.PrincipalTypeApplication = PRINCIPALTYPE.INSTITUTIONALMEMORY;
            input.IsFromProfile = true;

            managementProfileInputFilterDto = input;

            var responseListManagement = await _managementService.GetAllManagementsFiltered(input);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

            {
                // carga la data
                listManagement = responseListManagement.definition;
                var celling = Math.Ceiling((decimal)responseListManagement.definition.totalCount / 10);
                totalAcountPages = (int)celling;
                StateHasChanged();
                _spinnerService.Hide();
            }
            else
            {
                //error
                var message = responseListManagement != null && responseListManagement.response != null ?
                    responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
                _spinnerService.Hide();
            }

            _spinnerService.Hide();
        }

        public async Task downloadReportExcel()
        {
            _spinnerService.Show();
            StateHasChanged();
            managementProfileInputFilterDto.SkipCount = (actualPageIndex - 1) * 10;
            managementProfileInputFilterDto.MaxResultCount = 10;
            managementProfileInputFilterDto.PrincipalTypeApplication = PRINCIPALTYPE.INSTITUTIONALMEMORY;

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
    }
}
