using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ModalOnlyView
{

    public class ModalOnlyViewItemBase : ComponentBase
    {

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        private const long MaxFileSize = 10240000L; // 500 KB
        [NotNull]
        public Modal? ModalRef { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        public string Url { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();




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
        public async Task OpenModal(string url)
        {
            Url = url;
            StateHasChanged();
            await ModalRef.Show();
        }


    }


}
