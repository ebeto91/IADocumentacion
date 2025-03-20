using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListWorkTask
{
    public class ListWorkTaskBase : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }
        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;
        [Parameter]
        public GetWorkTaskForUsersAssignedDefinition? listWorktask { get; set; } = new GetWorkTaskForUsersAssignedDefinition();

        public WorkTaskInputDto workTaskInputDto { get; set; } = new WorkTaskInputDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelect = new List<SelectedItem>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IWorkTaskService _worktaskService { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }


        [Inject]
        public NavigationManager _navigation { get; set; }
        protected async override Task OnInitializedAsync()
        {

            //var listCatalog = await _catalogService.GetCatalogShowStatusProfile();
            var itemInputCatalog = new CatalogInputCollectionDto()
            {
                Collections = ["STATUS-MANAGEMENT"],
            };

            var listCatalog = await _catalogService.GetCatalogByFilters(itemInputCatalog);

            foreach (var item in listCatalog)
            {
                listSelect.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });


            }
            listSelect.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));




        }

        public string getStatusDisplay(string valueCatalog)
        {
            var itemSelect = listSelect.FirstOrDefault(x => x.Value == valueCatalog);
            if (itemSelect != null)
            {
                return itemSelect.Text;
            }
            return "";
        }

        public string getDateFormatDisplay(DateTime value)
        {
            return value.ToString("yyyy-MM-dd hh:mm tt");
        }

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
            workTaskInputDto.SkipCount = (actualPageIndex - 1) * 10;
            workTaskInputDto.MaxResultCount = 10;
            workTaskInputDto.UserAssignedId = userId;
            workTaskInputDto.IsVisible = true;


            var responseListWorkTask = await _worktaskService.GetWorkTaskForUsersAssignedFilter(workTaskInputDto);
            if (responseListWorkTask != null && responseListWorkTask.response != null && responseListWorkTask.response.Success)

            {
                // carga la data
                listWorktask = responseListWorkTask.definition;
                var celling = Math.Ceiling((decimal)responseListWorkTask.definition.totalCount / 10);
                totalAcountPages = (int)celling;
                StateHasChanged();
            }
            else
            {
                //error
                var message = responseListWorkTask != null && responseListWorkTask.response != null ?
                    responseListWorkTask.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
            }
        }

        public void goToProcessDetail(WorkTaskResponse workTask)
        {
            _navigation.NavigateTo($"/gestionCiudadana/tareas/detalle/{workTask.Id}");
        }
        #endregion
    }
}
