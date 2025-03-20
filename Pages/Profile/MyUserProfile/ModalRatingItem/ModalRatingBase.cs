using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ModalRatingItem
{
    public class ModalRatingBase : ComponentBase
    {

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IManagementService _managementService { get; set; }
        [NotNull]
        public Modal? ModalRef { get; set; }
        public ManagementProfileDto? _managementProfileDto { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }


        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task SendClasification(EditContext formContext)
        {
            if (_managementProfileDto.Rating == 0)
            {
                await _toastService.Warning($"Información", $"Debes colocar una clasificación, inténtalo de nuevo por favor", autoHide: true);
                return;
            }

            _spinnerService.Show();
            var response = await _managementService.UpdateRating(new RatingManagementDto()
            {
                Rating = _managementProfileDto.Rating,
                ManagementId = _managementProfileDto.Id
            });

            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Su clasificación ha sido recibida, muchas gracias por su participación.";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                await CloseModal();


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
            await ModalRef.Close();
            await ActionChild.InvokeAsync(null);
        }

        public async Task CloseModalSend()
        {
            await ModalRef.Close();
            await ActionChild.InvokeAsync(null);
        }

        public async Task OpenModal(ManagementProfileDto managementProfileDto)
        {
            _managementProfileDto = managementProfileDto;
            StateHasChanged();
            await ModalRef.Show();
        }
    }
}
