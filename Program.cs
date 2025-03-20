using Blazored.LocalStorage;
using BlazorSpinner;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.HttpClientExtensions;
using System.Globalization;

namespace RAS823_MC_CiudadMunicipal_FrontEnd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");


            builder.Services.AddBlazoredLocalStorage();
            // Add this line of code
            builder.Services.AddBootstrapBlazor(null, options =>
            {
                // Ignore the loss of localized key-value culture information
                options.IgnoreLocalizerMissing = true;
            });
            builder.Services.AddSweetAlert2();

            //spinner
            builder.Services.AddScoped<SpinnerService>();

            //await SetCultureAsync(builder);

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISurveyService, SurveyService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<ICitizenManagment, CitizenManagment>();
            builder.Services.AddScoped<IInstitutionalMemoryService, InstitutionalMemoryService>();
            builder.Services.AddScoped<HashService>();
            builder.Services.AddTransient<IGZipHelper, GZipHelper>();
            builder.Services.AddScoped<ICatalogService, CatalogService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ICatalogAutomaticResponseService, CatalogAutomaticResponseService>();
            builder.Services.AddScoped<IAssociationService, AssociationService>();
            builder.Services.AddScoped<IDistrictService, DistrictService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IManagementService, ManagementService>();
            builder.Services.AddScoped<IWorkTaskService, WorkTaskService>();
            builder.Services.AddScoped<IWorkTaskCommentService, WorkTaskCommentService>();
            builder.Services.AddScoped<IWorkTaskHistoryService, WorkTaskHistoryService>();
            builder.Services.AddScoped<IDownloadExcelService, DownloadExcelService>();
            builder.Services.AddScoped<IHolidaysService, HolidaysService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IConfigSettingService, ConfigSettingService>();
          
            builder.Services.AddScoped<IExcelService, ExcelService>();
            builder.Services.AddTransient<CustomHeaderHandler>();
            builder.Services.AddScoped<CustomAuthService>();

            var url = builder.Configuration["BaseAddress:Url"];
            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<CustomHeaderHandler>();
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url)
                };
                return httpClient;
            }
          );
            builder.Services.AddScoped<IValidationRouteService, ValidationRouteService>();


            //builder.Services.AddMsalAuthentication(options =>
            //{
            //    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            //});

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthorizationCore();



       

            builder.Services.AddMsalAuthentication<RemoteAuthenticationState,
                CustomUserAccount>(options =>
                {
                    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomAccountFactory>();

            builder.Services.AddApiAuthorization<RemoteAuthenticationState,
                CustomUserAccount>(options =>
                {

                }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomAccountFactory>();


            await builder.Build().RunAsync();
        }


        static async Task SetCultureAsync(WebAssemblyHost host)
        {

            //var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
            //var cultureName = await jsRuntime.GetCulture();

            //if (!string.IsNullOrEmpty(cultureName))
            //{
            //    var culture = new CultureInfo(cultureName);


            //    CultureInfo.DefaultThreadCurrentCulture = culture;
            //    CultureInfo.DefaultThreadCurrentUICulture = culture;
            //}
        }
    }
}
