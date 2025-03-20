using BlazorSpinner;
using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ModalRatingItem;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListComplaints
{
    public class ListComplaintBase : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }
        [Parameter]
        public string associationId { get; set; }
        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;
        [Parameter]
        public ModelProfileManagementDefinition? listManagement { get; set; } = new ModelProfileManagementDefinition();

        public ManagementProfileInputFilterDto managementProfileInputFilterDto { get; set; } = new ManagementProfileInputFilterDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelect = new List<SelectedItem>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IManagementService _managementService { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }
        protected async override Task OnInitializedAsync()
        {

            var listCatalog = await _catalogService.GetCatalogShowStatusProfile();

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
            managementProfileInputFilterDto.SkipCount = (actualPageIndex - 1) * 10;
            managementProfileInputFilterDto.MaxResultCount = 10;
            managementProfileInputFilterDto.AssociationRelatedMemoryId = associationId;
            managementProfileInputFilterDto.CreatedUserId = userId;
            managementProfileInputFilterDto.PrincipalTypeApplication = PRINCIPALTYPE.CORRUPTION;
            managementProfileInputFilterDto.TypeCreation = "EXTERNAL";


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

        public void goToProcessDetail(ManagementProfileDto managementProfileDto)
        {
            _navigation.NavigateTo($"/miperfil/detalle/denuncia/{managementProfileDto.Id}");
        }
        #region modal
        public ModalRating ModalRatingRef { get; set; }

        protected async Task OpenModal(ManagementProfileDto managementProfileDto)
        {
            await ModalRatingRef.OpenModal(managementProfileDto);
        }

        protected async Task ActionComponentFather()
        {
            await OnPageClick(actualPageIndex);
        }
        #endregion
    }
}
