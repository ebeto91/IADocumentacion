using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using BlazorSpinner;
using BootstrapBlazor.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using Microsoft.IdentityModel.Tokens;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Login.AccessDirectiveCallback
{
    public class AccessDirectiveCallbackBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        [Inject]
        public IConfiguration _configuration { get; set; }
        [Inject]
        public CustomAuthService customAuthService { get; set; }
        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public string preferredUsernameClaim { get; set; }
        public bool isReadyData { get; set; }

        protected override async Task OnAfterRenderAsync(bool isFirstRender)
        {
            if (isFirstRender)
            {
                BlazorSetTimeout(1000, async () =>
                {
                    var t = await AuthState;
                    var c = t.User.Identity.IsAuthenticated;

                    preferredUsernameClaim = t.User.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username")).Value;
                    var preferredUsernameClaim2 = t.User.Claims.FirstOrDefault(c => c.Type.Equals("oid"));

                    isReadyData = true;
                    StateHasChanged();

                    var securityKey = _configuration["Security:Key"];

                    string dataCrypted = await _jsRuntime.InvokeAsync<string>("encryptData", securityKey, preferredUsernameClaim);
                    //_spinnerService.Show();
                    var responser = await _userService.LoginUserAccessDirective(new UserInfoAccessDirective()
                    {
                        Information = dataCrypted,
                    });

                    BlazorSetTimeout(500, async () =>
                    {
                        await _jsRuntime.InvokeAsync<string>("removeAllStorageKeys");

                        if (responser != null && responser.response.Success)
                        {

                            _spinnerService.Hide();
                            Navigation.NavigateTo("/miperfil", true);
                        }
                        else
                        {
                            _spinnerService.Hide();
                            var message = responser != null && responser.response != null ? responser.response.Message : "Problemas al iniciar sesion";
                            await ToastService.Error("Inicio Sesión", message, autoHide: true);
                        }

                    });
                });
            }
        }

        public void ConsultUser(AuthenticationState authenticationState)
        {

        }

        public async Task Data()
        {


            var t = await AuthState;
            var c = t.User.Identity.IsAuthenticated;

            var preferredUsernameClaim = t.User.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"));
            var preferredUsernameClaim2 = t.User.Claims.FirstOrDefault(c => c.Type.Equals("oid"));

            //GraphServiceClient gr = new GraphServiceClient();
            //var users = gr.
            // if (preferredUsernameClaim != null)
            // {
            //     return user.Claims.FirstOrDefault(p => p.Type.Equals("name")).Value;
            // }

        }
        public void BlazorSetTimeout(int miliseconds, Action action)
        {
            Task.Delay(miliseconds)
                .ContinueWith(previousTask =>
                {
                    action.Invoke();

                });
        }

    }
}
