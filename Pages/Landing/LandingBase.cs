using BlazorSpinner;
using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Landing
{
    public class LandingBase:ComponentBase
    {
        [Inject]
        public ISurveyService _surveyService { get; set; }

        [Inject]
        public Blazored.LocalStorage.ILocalStorageService LocalStorage {  get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public  List<List<SurveyResponseDataCarrousel>> surveyResponseDataCarrousels = new List<List<SurveyResponseDataCarrousel>>();


        protected async override Task OnInitializedAsync()
        {

            var token = await LocalStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo($"/miperfil", true);

            }
            else
            {


                SurveyInputExternalDto surveyInputExternalDto = new SurveyInputExternalDto();

                surveyInputExternalDto.DueRate = DateTime.Today;

                var result = await _surveyService.GetSurveyFilter(surveyInputExternalDto);
                if (result != null)
                {
                    surveyResponseDataCarrousels = result.definition
                .Select((item, index) => new { item, index })
                .GroupBy(x => x.index / 3)
                .Select(g => g.Select(x => x.item).ToList())
                .ToList();
                }
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
              

               
            
            }
        }
    }
}
