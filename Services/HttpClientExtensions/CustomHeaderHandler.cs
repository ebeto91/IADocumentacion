using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using Blazored.LocalStorage;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using BootstrapBlazor.Components;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.HttpClientExtensions
{
    public class CustomHeaderHandler : DelegatingHandler
    {
        public CustomHeaderHandler(ILocalStorageService localStorageService, CustomAuthService customAuthService, ToastService toastService) : base(new HttpClientHandler())
        {
            _localStorage = localStorageService;
            _customAuthService = customAuthService;
            _toastService = toastService;
        }

        public Blazored.LocalStorage.ILocalStorageService _localStorage { get; set; }
        public CustomAuthService _customAuthService { get; set; }
        public ToastService _toastService { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Accept-Language", "es-CR");


            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (token != null)
            {
                if (!_customAuthService.IsTokenExpired(token))
                {
                    request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
                }
                else
                {
                    await _customAuthService.Logout();
                }

            }


            return await base.SendAsync(request, cancellationToken);

        }


    }
}
