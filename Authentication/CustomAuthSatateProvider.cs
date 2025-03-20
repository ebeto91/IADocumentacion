using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Nodes;


namespace RAS823_MC_CiudadMunicipal_FrontEnd.Authentication
{
    public class CustomAuthSatateProvider : AuthenticationStateProvider
    {

        IEnumerable<Claim> claims;
        [Inject]
        Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; }


        public CustomAuthSatateProvider()
        {
         
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            try
            {
               var token = await LocalStorage.GetItemAsStringAsync("authToken");
                if (!string.IsNullOrEmpty(token))
                {

                    if (!IsTokenExpired(token))
                    {

                        var identity = GetClaims(token);
                        var userToken = new ClaimsPrincipal(identity);
                        return new AuthenticationState(userToken);
                    }
                    else
                    {
                        var tokenDelete = await LocalStorage.GetItemAsStringAsync("authToken");
                        if (!string.IsNullOrEmpty(tokenDelete))
                        {
                            await LocalStorage.RemoveItemAsync("authToken");
                        }


                        return new AuthenticationState(user);
                    }

                  
                }
                else
                {

                    return new AuthenticationState(user);
                }
               
            }
            catch(Exception ex)
            {
          
            }

            return new AuthenticationState(user);
        }

        public void UserIsAuthenticated()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public ClaimsIdentity GetClaims (string token)
        {
            
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtSecurityToken = handler.ReadJwtToken(token);
                claims = jwtSecurityToken.Claims;
            }

            return new ClaimsIdentity(claims,"jwt");
        }

        public bool IsTokenExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;
            var fecha= DateTime.UtcNow;
            var result = expiration < fecha;
            return result;
        }

    }
}
