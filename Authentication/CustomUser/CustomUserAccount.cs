using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser
{
    public class CustomUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("amr")]
        public string[]? AuthenticationMethod { get; set; }
    }

    public class CustomAccountFactory(NavigationManager navigation,
     IAccessTokenProviderAccessor accessor, CustomAuthService customAuthService)
     : AccountClaimsPrincipalFactory<CustomUserAccount>(accessor)
    {
        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
            CustomUserAccount account, RemoteAuthenticationUserOptions options)
        {


            // Si MSAL no tiene un usuario, intentamos recuperar el login manual
            var manualUser = await customAuthService.GetUserAsync();
            if (manualUser.Identity?.IsAuthenticated == true)
            {
                return manualUser;
            }

            // Intentamos obtener el usuario autenticado con MSAL
            var msalUser = await base.CreateUserAsync(account, options);

            // Si el usuario de MSAL está autenticado, usamos ese
            if (msalUser.Identity?.IsAuthenticated == true)
            {
                var userIdentity = (ClaimsIdentity)msalUser.Identity;

                // Agregamos los claims de autenticación
                if (account.AuthenticationMethod is not null)
                {
                    foreach (var value in account.AuthenticationMethod)
                    {
                        userIdentity.AddClaim(new Claim("amr", value));
                    }
                }

                return msalUser;
            }
            // Si ninguno está autenticado, devolvemos un usuario anónimo
            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }




}
