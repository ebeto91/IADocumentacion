using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalComplaint.ControlInternalComplaintEdit.ControlInternalComplaintEditModal
{
    public class ControlInternalComplaintEditModalBase : ComponentBase
    {
        [Inject]
        public IManagementService _managementService { get; set; }

        [Parameter]
        public FullManagementDto PrincipalObject { get; set; } = new FullManagementDto();

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        [Parameter]
        public EventCallback ActionComplete { get; set; }
        [Parameter]

        public List<SelectedItem> listDefaultResponseResolve { get; set; }
        [Parameter]
        public List<CatalogAutomaticResponseDto> listCatalogAutomaticResponse { get; set; }
        [Parameter]
        public SelectedItem selectedItem { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }
        public async Task HandleFormValid()
        {
            if (string.IsNullOrEmpty(PrincipalObject.ResolutionReason.Trim()))
            {
                await _toastService.Error("Información requerida", "El mensaje de resolución es requerido", autoHide: true);
                return;
            }

            _spinnerService.Show();

            //var itemListSelected = listCatalog.FirstOrDefault(x => x.Code == managementDepartmentDto.Code);

            //managementDepartmentDto.Collection = itemListSelected != null ? itemListSelected.Collection : "";}

            var handleManangement = PrincipalObject.ToJson().FromJson<HandleManagementTaskDto>();
            handleManangement.ManagementId = PrincipalObject.Id;

            var response = await _managementService.AproveManagement(handleManangement);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Solicitud resuelta con éxito, se estará notificando a los usuarios";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);

                await ActionComplete.InvokeAsync(null);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

        }

        public async Task CloseModal()
        {
            await ActionChild.InvokeAsync(null);
        }

        public async Task OnItemChanged(SelectedItem item)
        {
            if (item != null)
            {
                if (item.Value != "")
                {
                    var guid = Guid.Parse(item.Value);
                    var itemAutomaticResponse = listCatalogAutomaticResponse.FirstOrDefault(x => x.Id == guid);
                    if (itemAutomaticResponse != null)
                    {
                        PrincipalObject.ResolutionReason = itemAutomaticResponse.Description;
                    }


                }
                else
                {
                    PrincipalObject.ResolutionReason = "";
                }
            }
            StateHasChanged();
        }

        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("/gestionCiudadana/solicitudes");
        }
        #endregion
    }
}
