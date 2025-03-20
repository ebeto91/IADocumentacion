using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Menu;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Security.Claims;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Layout
{
    public class MainBase : LayoutComponentBase
    {
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public CustomAuthService _customAuthService { get; set; }


        [Inject]
        public IUserService _userService { get; set; }

        [Inject]
        public IMenuService _menuService { get; set; }

        public List<MenuDto> menuDtos = new List<MenuDto>();

        public string name { get; set; }
        public string roleName { get; set; }
        public string userId { get; set; }
        [Inject]
        public NavigationManager Navigation { get; set; }




        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();
            var claimList = await _customAuthService.GetClaims();
            if (claimList is not null)
            {

                var uniqueNameClaim = claimList.Claims.FirstOrDefault(c => c.Type == "unique_name");
                name = uniqueNameClaim?.Value;

                var roles = claimList.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                var role = roles[0].Value;
                roleName = role ?? string.Empty;

                var userIdUnique = claimList.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdUnique != null)
                {
                    userId = userIdUnique.Value ?? string.Empty;
                }

                var result = await _menuService.GetMenuFilters(role);
                if (result is not null && result.response.Success)
                {
                    menuDtos = result.definition;
                }
            }

            _spinnerService.Hide();
            //return base.OnInitializedAsync();
        }



        public async Task HandleLogout()
        {
            await _customAuthService.Logout();
        }

        public void HandleNav(string link)
        {
            Navigation.NavigateTo($"/{link}");
        }
    }
}
