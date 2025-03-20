using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using NPOI.OpenXml4Net.OPC.Internal;
using System.Security.Claims;
using BootstrapBlazor.Components;
using System.Diagnostics.CodeAnalysis;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers.HtmlAttributes
{
    public class AuthorizeScopeAttribute : Attribute
    {
        public string Scope { get; }

        public AuthorizeScopeAttribute(string scope)
        {
            Scope = scope;
        }
    }

    public class AuthorizeScopeDirective : ComponentBase
    {
        [Inject]
        private CustomAuthService _customAuthService { get; set; }

        [Parameter]
        public string Scope { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IGZipHelper gZipHelper { get; set; }

        private bool IsAuthorized { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userTokenData = await _customAuthService.GetClaims();
            if (userTokenData != null)
            {
                var scopeClaim = userTokenData.Claims.FirstOrDefault(x => x.Type == CustomClaims.Scope);
                if (scopeClaim != null)
                {

                    var listScopes = gZipHelper.DecompressData(scopeClaim.Value);
                    var scopes = JsonConvert.DeserializeObject<List<string>>(listScopes);

                    var listClaimsValid = Scope.Split(',');

                    bool scopeValid = scopes.Any(item => listClaimsValid.Any(scope => scope == item));

                    IsAuthorized = scopeValid;
                }
            }
        
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (IsAuthorized)
            {
                builder.AddContent(0, ChildContent);
            }
        }
    }
}
