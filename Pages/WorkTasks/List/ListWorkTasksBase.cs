using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.Download;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskKanban;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.List
{
    public class ListWorkTasksBase : ComponentBase
    {
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public IWorkTaskService _workTaskService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        public WorkTaskKanbanInputDto workTaskKanbanInputDto { get; set; } = new WorkTaskKanbanInputDto();
        public KanbanResponseDefinition kanbanResponseDefinition { get; set; } = new KanbanResponseDefinition();

        [Inject]
        public ICatalogService _catalogService { get; set; }
        public IEnumerable<Catalog> _catalogList { get; set; }




        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();



        public List<Catalog> listCatalogData = new List<Catalog>();

        [Inject]
        public IManagementService _managementService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IDownloadExcelService _downloadService { get; set; }


        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                    "./Pages/WorkTasks/List/ListWorkTasks.razor.js");

                //await module.InvokeVoidAsync("addHandlers");
            }
        }

        async ValueTask DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }

        protected async override Task OnInitializedAsync()
        {

            _spinnerService.Show();
            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "TYPE-PROCCESS"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;


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
            var listSubtype = listCatalogData.Where(x => x.Collection == "TYPE-PROCCESS");
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



            await SearchDataCall();

            _spinnerService.Hide();

        }
        public async Task changeSizeColumn(string columnId)
        {
            await module.InvokeVoidAsync("changeSizeColumn", columnId);
        }


        #region create new 

        public async Task createNewInternal()
        {
            _spinnerService.Show();
            _navigation.NavigateTo($"/gestionCiudadana/solicitudes/internas/crear/tarea");
            _spinnerService.Hide();
        }

        public async Task goToProcessEdit(string workTaskId)
        {
            _spinnerService.Show();
            _navigation.NavigateTo($"/gestionCiudadana/tareas/detalle/{workTaskId}");
            _spinnerService.Hide();
        }


        #endregion

        #region search
        public async Task searchData()
        {
            _spinnerService.Show();

            await SearchDataCall();

            _spinnerService.Hide();
        }


        private async Task SearchDataCall()
        {


            workTaskKanbanInputDto.IsVisible = true;
            var responseListManagement = await _workTaskService.GetWorkTaskFilterForKanban(workTaskKanbanInputDto);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

            {
                // carga la data
                kanbanResponseDefinition = responseListManagement.definition;
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
        #region download worktask
        public async Task downloadReportExcel()
        {
            _spinnerService.Show();
            StateHasChanged();

            var workTaskDownloadDto = workTaskKanbanInputDto.ToJson().FromJson<WorkTaskDownloadDto>();
            workTaskDownloadDto.IsVisible = true;
            var responseListManagement = await _downloadService.GetAllWorkTaskFiltered(workTaskDownloadDto);
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


        public List<WorkTaskResponse> getListByHeaderCode(string code)
        {
            List<WorkTaskResponse> workTaskForKanbans = new List<WorkTaskResponse>();


            var itemDefinition = kanbanResponseDefinition.ListWorkTaskGroup.FirstOrDefault(x => x.Code == code);
            if (itemDefinition != null)
            {
                workTaskForKanbans = itemDefinition.ListWorkTask;
            }



            return workTaskForKanbans;
        }


        public string findDisplayValue(string code)
        {
            var itemFindCatalog = listCatalogData.FirstOrDefault(x => x.Code == code);

            if (itemFindCatalog != null )
            {
                return itemFindCatalog.DisplayLabel;
            }
            return "Sin asignar";
        }
        public Color findColor(string code)
        {
            switch (code) {
                case PRIORITY.LOW:
                    return Color.Success;
                case PRIORITY.MID:
                    return Color.Warning;
                case PRIORITY.HIGH:
                    return Color.Danger;
                default:
                    return Color.None;
            }

        }
    }
}
