using Blazored.LocalStorage;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ChangePassword
{
    public class ChangePasswordBase:ComponentBase
    {
        public string Id { get; set; }
        public ChangePasswordDto changePasswordDto = new ChangePasswordDto();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IUserService _userService { get; set; }

        [Inject]
        public IGZipHelper gZipHelper { get; set; }

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        public Blazored.LocalStorage.ILocalStorageService LocalStorage {  get; set; }

        protected override void OnInitialized()
        {
            // Retrieve the query parameters from the URL
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            // Access the "id" query parameter
            if (queryParams.TryGetValue("id", out var idValue))
            {
                Id = idValue;
                var des = gZipHelper.DecompressDataUrl(Id);
                var changePasswordReset = des.FromJson<ChangePasswordResetModel>();

                changePasswordDto.PasswordExpiredDate = changePasswordReset.PasswordExpiredDate;
                changePasswordDto.ResetCodeByUrl = changePasswordReset.PasswordResetCode;
                changePasswordDto.Id = changePasswordReset.Id;

            }
        }


        public async Task HandleChangePassword()
        {
            var responser = await _userService.ChangePassword(changePasswordDto);
            if (responser != null && responser.response.Success)
            {
              
                var token = await LocalStorage.GetItemAsync<string>("authToken");
                if(!string.IsNullOrEmpty(token))
                {
                    
                    await LocalStorage.RemoveItemAsync("authToken");
                }
                await ToastService.Success("Cambio de Contraseña", responser.response.Message, autoHide: true);
                NavigationManager.NavigateTo("/login");

            }
            else
            {
                await ToastService.Error("Cambio de Contraseña", responser.response.Message, autoHide: true);
            }
        }
    }

    
}
