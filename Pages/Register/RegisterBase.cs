using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Register
{
    public class RegisterBase: ComponentBase
    {
        public UserRegisterDto userRegisterDto = new UserRegisterDto();
        public int counterCharacters = 0;
        public bool isFormValid = true;

        /*     public IEnumerable<SelectedItem> items { get; set; } = new SelectedItem[]
        {
             new() { Text = "Item 1", Value = "" },
             new() { Text = "Item 2", Value = "2" },
             new() { Text = "Item 3", Value = "3" }
        };*/
      

        public IEnumerable<SelectedItem> items;

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }


        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        [Inject]
        public IUserService _userService { get; set; }


        [Inject]
        public NavigationManager Navigation { get; set; }


        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();

            List<SelectedItem> listSelect = new List<SelectedItem>();
            var list = await _catalogService.GetCatalogByCollection("TYPE-IDENTIFICATION");
            
             foreach (var item in list)
             {
                 listSelect.Add(new SelectedItem()
                 {
                     Text = item.DisplayLabel,
                     Value = item.Code,
                 });

                
             }
            listSelect.Insert(0, (new SelectedItem { Text = "Seleccione el tipo de cédula", Value = "" }));
            items = listSelect;

            _spinnerService.Hide();
            //return base.OnInitializedAsync();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                    "./Pages/Register/Register.razor.js");

                await module.InvokeVoidAsync("checkLengthAndTrim");
            }
        }

        async ValueTask DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
        public async Task HandleRegister()
        {

            _spinnerService.Show();
            var responser = await _userService.CreateUserRegister(userRegisterDto);
                int x = 0;
                if (responser != null && responser.response.Success)
                {
                    await ToastService.Success("Registro", responser.response.Message, autoHide: true);
                    Navigation.NavigateTo("/login");

            }
            else
            {
                await ToastService.Error("Registro", responser.response.Message, autoHide: true);
                _spinnerService.Hide();
            }
            
        }

        public void HandleInvalidSubmit()
        {
            // Keep the errors visible and trigger re-render
            StateHasChanged();
           
        }



        public async Task OnItemChanged(SelectedItem item)
        {
            userRegisterDto.Identification = "";
            StateHasChanged();
            
        }
    }
}
