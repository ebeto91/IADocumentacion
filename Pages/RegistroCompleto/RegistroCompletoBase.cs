using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.RegistroCompleto
{
    public class RegistroCompletoBase:ComponentBase
    {
        public string Id { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IUserService _userService { get; set; }

       public RegisterCompleteDto registerCompleteDto = new RegisterCompleteDto();

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        protected async override void OnInitialized()
        {
            // Retrieve the query parameters from the URL
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            // Access the "id" query parameter
            if (queryParams.TryGetValue("id", out var idValue))
            {
                Id = idValue;

                if (!string.IsNullOrEmpty(Id))
                {
                    _spinnerService.Show();

                    registerCompleteDto.IdUser = Id;

                    var result= await _userService.CompleteRegister(registerCompleteDto);
                    if(result != null)
                    {
                        if(result.response.Success)
                        {

                            _spinnerService.Hide();
                            await ToastService.Success("Registro", result.response.Message, autoHide: true);
                            Navigation.NavigateTo("/login");
                        }
                        else
                        {
                            _spinnerService.Hide();
                            await ToastService.Error("Registro", result.response.Message, autoHide: true);
                        }
                        

                    }
                    else
                    {

                        _spinnerService.Hide();
                        await ToastService.Error("Registro", "Error al completar el registro", autoHide: true);
                    }

                }

            }
        }
    }
}
