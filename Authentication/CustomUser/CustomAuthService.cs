using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser
{
    public class CustomAuthService
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

        public event Action? UserChanged;
        public NavigationManager _navigation;
        public CustomAuthService(ILocalStorageService localStorage, NavigationManager navigationManager)
        {
            _localStorage = localStorage;
            _navigation = navigationManager;
        }

        public async Task<ClaimsPrincipal> GetUserAsync()
        {
            if (_currentUser.Identity?.IsAuthenticated == true)
                return _currentUser;

            var result = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(result))
            {
                if (!IsTokenExpired(result))
                {
                    _currentUser = CreateUserFromToken(result);
                }
                else
                {
                    await Logout();
                }
               
            }

            return _currentUser;
        }

        public async Task LoginAsync(string token)
        {

            var claims = new List<Claim>
            {
              new Claim("amr", "custom-login"),
              new Claim("token", token)
            };

            var identity = new ClaimsIdentity(claims, "CustomAuth");
            _currentUser = new ClaimsPrincipal(identity);

            // Guardar token en el LocalStorage
            await _localStorage.SetItemAsStringAsync("authToken", token);

            UserChanged?.Invoke();

        }

        private ClaimsPrincipal CreateUserFromToken(string token)
        {
            var claims = new List<Claim>
            {
                new Claim("amr", "custom-login"),
                new Claim("token", token)
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));
        }

        public async Task Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            await _localStorage.RemoveItemAsync("authToken");
            _navigation.NavigateTo($"/", true);
            UserChanged?.Invoke();
        }




        public async Task<ClaimsIdentity> GetClaims()
        {

            var userAuth = await GetUserAsync();
            if (userAuth != null && userAuth.Claims != null)
            {
                var token = userAuth.Claims.FirstOrDefault(x => x.Type == "token");
                if (token == null)
                {
                    return null;
                }


                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token.Value))
                {
                    var jwtSecurityToken = handler.ReadJwtToken(token.Value);
                    return new ClaimsIdentity(jwtSecurityToken.Claims, "jwt");
                }

                return null;
            }
            else
            {
                return null;
            }


        }

        public bool IsTokenExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;
            var fecha = DateTime.UtcNow;
            var result = expiration < fecha;
            return result;
        }

    }

}
