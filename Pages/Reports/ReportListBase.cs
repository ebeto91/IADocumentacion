using BlazorSpinner;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Microsoft.JSInterop;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Report;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Security.Principal;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Reports
{
    public class ReportListBase : ComponentBase
    {
        [Inject]
        public IConfiguration _configuration { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        [Inject]
        private IReportService _reportService { get; set; }


        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public HashService _hashService { get; set; }
        [Inject]
        public NavigationManager Navigation { get; set; }

        private IJSObjectReference? module;


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                



              
            }
        }

        protected override async Task OnInitializedAsync()
        {

            this._spinnerService.Show();

            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
            "./Pages/Reports/ReportList.razor.js");


            var securityKey = _configuration["Security:Key"];

            var response = await _reportService.GetInformationReport();
            if (response != null && response.response.Success)
            {
                var baseDecode = _hashService.Base64Decode(response.definition);
                string dataDescripted = await _jsRuntime.InvokeAsync<string>("decryptData", securityKey, baseDecode);


                var itemDataReport = dataDescripted.FromJson<ReportInformationDto>();


                await module.InvokeVoidAsync("embedReport", "embed-container", itemDataReport.Id, itemDataReport.EmbedUrl, itemDataReport.Token);
                this._spinnerService.Hide();
                StateHasChanged();
            }
            else
            {

                Navigation.NavigateTo("/miperfil");
                this._spinnerService.Hide();
            }


        }
    }
}
