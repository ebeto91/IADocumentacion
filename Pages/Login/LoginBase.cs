using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.ConfigSetting;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        public UserCredenditialsDto userCredenditialsDto = new UserCredenditialsDto();

        public bool isChecked = false;


        [Inject]
        public NavigationManager Navigation { get; set; }

        public RegeneratePasswordDto regeneratePasswordDto = new RegeneratePasswordDto();
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public IConfigSettingService _configSettingService { get; set; }
        [Inject]
        public IConfiguration _configuration { get; set; }
        [Inject]
        public HashService _hashService { get; set; }
        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [NotNull]
        public Modal? Modal { get; set; }

        [NotNull]
        public Modal? LargeModal { get; set; }


        public string validationMessage = string.Empty;

        public string domainConfig = string.Empty;

        public bool showDataLoginNormal = true;

        private EditContext editContext;

        [Inject]
        public Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _spinnerService.Show();
                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                    "./Pages/Login/Login.razor.js");

                //await module.InvokeVoidAsync("addHandlers");
               var obj = await LocalStorage.GetItemAsStringAsync("rememberApp");
                if (!string.IsNullOrEmpty(obj))
                {

                    await module.InvokeVoidAsync("ExistCheck");
                }
                var securityKey = _configuration["Security:Key"];

          
                var response = await _configSettingService.GetSettingsByName(new Dto.ConfigSetting.ConfigSettingInput()
                {
                    NameFilter = "DOMAINCONFIG"
                });
        
                if (response != null && response.response.Success)
                {
                    var domainConfigDecoded = _hashService.Base64Decode( response.definition);
                    string dataCrypted = await _jsRuntime.InvokeAsync<string>("decryptData", securityKey, domainConfigDecoded);


                    var infoData = dataCrypted.FromJson<ConfigSettingModel>();
                    //formato esperado muni-carta\.go\.cr
                    var output = infoData.Value.Replace(".", @"\.");
                    domainConfig = output;
                    _spinnerService.Hide();
                  
                }
                else
                {
                    _spinnerService.Hide();
                    
                }

            }

        }

        public void UpdateCharacterCount(ChangeEventArgs e)
        {
            var input = e.Value?.ToString() ?? string.Empty;

            string pattern = $@"^[^@]+@{domainConfig}$";

            if (Regex.IsMatch(input, pattern))
            {
                showDataLoginNormal=false;
            }
            else
            {
                showDataLoginNormal = true;
            }
            StateHasChanged();
        }

        // Constructor to initialize EditContext
        protected async override void OnInitialized()
        {
            editContext = new EditContext(regeneratePasswordDto);
            var obj = await LocalStorage.GetItemAsStringAsync("rememberApp");
            if (!string.IsNullOrEmpty(obj))
            {
                userCredenditialsDto = JsonSerializer.Deserialize<UserCredenditialsDto>(obj);
                StateHasChanged();
            }
        }

        public async void OnCheckboxChanged()
        {
          //  if(string.IsNullOrEmpty(userCredenditialsDto.EmailAddress) || string.IsNullOrEmpty(userCredenditialsDto.Password)){
                if (module != null)
                {
                   await module.InvokeVoidAsync("CheckBoxRemenber");
                }
           // }
           /* else
            {
                var obj= JsonSerializer.Serialize(userCredenditialsDto);
                var token = await LocalStorage.GetItemAsStringAsync("rememberApp");
                if (!string.IsNullOrEmpty(token))
                {

                    await LocalStorage.RemoveItemAsync("rememberApp");
                }

                await LocalStorage.SetItemAsStringAsync("rememberApp", obj);

            }*/
            
        }

       /* public void OnCheckboxChanged()
        {
            //message = $"Checkbox changed to {(isChecked ? "Checked" : "Unchecked")}.";
        }*/

        public void OnCheckbox(ChangeEventArgs e)
        {
           var r= (bool)e.Value ? "Checked" : "Unchecked";
           
        }

        public async Task<bool> OnSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(regeneratePasswordDto.EmailAddress))
            {
                // Show validation message if email is empty
                validationMessage = "Por favor, ingrese su correo electrónico.";
                return false;
            }
            await Task.Delay(1000);
            return true;
        }

        public async Task HandleRegeneratePassword()
        {
            _spinnerService.Show();
            var responser = await _userService.UserChangePassword(regeneratePasswordDto);
            if (responser != null && responser.response.Success)
            {
                _spinnerService.Hide();
                await ToastService.Success("Cambio de Contraseña Exitoso", responser.response.Message, autoHide: true);
            }
            else
            {
                _spinnerService.Hide();
                await ToastService.Error("Error Cambio de Contraseña", responser.response.Message, autoHide: true);
            }

            await Modal.Close();
        }

        public async Task OpenModal()
        {
            ResetForm();
            regeneratePasswordDto.EmailAddress = null;
            validationMessage = "";
            await Modal.Toggle();
        }

     

        private void ResetForm()
        {
            // Reset the model properties
            regeneratePasswordDto = new RegeneratePasswordDto();

            // Optionally, reset the validation state
            editContext = new EditContext(regeneratePasswordDto);

            // Clear validation message (if any)
            validationMessage = string.Empty;
        }

        async ValueTask DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
        public async Task HandleLogin(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;

            _spinnerService.Show();
            var responser = await _userService.LoginUser(userCredenditialsDto);
            if (responser!=null && responser.response.Success)
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
        }

        public async Task HandleLoginAzureDirective(EditContext formContext)
        {

            //_spinnerService.Show();

            InteractiveRequestOptions requestOptions = new()
            {
                Interaction = InteractionType.SignIn,
                ReturnUrl = "/weather",
            };

            var a = Uri.EscapeDataString(Navigation.Uri);
            Navigation.NavigateToLogin("authentication/login", requestOptions);
        }
    }
}
