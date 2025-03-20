using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services
{
    public interface IValidationRouteService
    {
        public Task<bool> HasAccessRoute(string scopes);
        public Task<int> HowManyPermissionsHave(string scopes);

    }

    class ValidationRouteService : IValidationRouteService
    {
        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        [Inject]
        public IGZipHelper _gZipHelper { get; set; }


        public ValidationRouteService(CustomAuthService customAuthService, IGZipHelper gZipHelper)
        {
            _customAuthService = customAuthService;
            _gZipHelper = gZipHelper;

        }
        public async Task<bool> HasAccessRoute(string scopeList)
        {
            var userTokenData = await _customAuthService.GetClaims();


            bool scopeValid = false;
            if (userTokenData != null)
            {

                var scopeClaim = userTokenData.Claims.FirstOrDefault(x => x.Type == CustomClaims.Scope);
                if (scopeClaim != null)
                {

                    var listScopes = _gZipHelper.DecompressData(scopeClaim.Value);
                    var scopes = JsonConvert.DeserializeObject<List<string>>(listScopes);

                    var listClaimsValid = scopeList.Split(',');

                    scopeValid = scopes.Any(item => listClaimsValid.Any(scope => scope == item));

                    return scopeValid;
                }
                else
                {
                    return scopeValid;
                }
            }
            else
            {
                return scopeValid;
            }

        }

        public async Task<int> HowManyPermissionsHave(string scopeList)
        {
            var userTokenData = await _customAuthService.GetClaims();
            if (userTokenData == null)
            {
                return 0;
            }


            var scopeClaim = userTokenData.Claims.FirstOrDefault(x => x.Type == CustomClaims.Scope);
            if (scopeClaim != null)
            {

                var listScopes = _gZipHelper.DecompressData(scopeClaim.Value);
                var scopes = JsonConvert.DeserializeObject<List<string>>(listScopes);

                var listClaimsValid = scopeList.Split(',');

                var scopeValid = scopes.Where(item => listClaimsValid.Any(scope => scope == item));

                return scopeValid.Count();
            }
            else
            {
                return 0;
            }



        }
    }
}
