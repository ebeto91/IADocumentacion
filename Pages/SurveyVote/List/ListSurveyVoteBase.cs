using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVote.List
{
    public class ListSurveyVoteBase:ComponentBase
    {
        [Parameter]
        public bool autoLoad { get; set; } = false;


        [Parameter]
        public bool SurveyVoteRegisterUserInternal { get; set; } = true;
        public ModelSurveyVoteDefinition ModelDefinition { get; set; } = new ModelSurveyVoteDefinition();

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;

        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]

        public ISurveyService _surveyService { get; set; }

        public SurveyInputDto surveyVoteInputFilterDto { get; set; } = new SurveyInputDto();

        protected async override Task OnInitializedAsync()
        {
            if (SurveyVoteRegisterUserInternal)
            {
                surveyVoteInputFilterDto.TypeCreation = TYPE_PROCCESS_SURVEY.INTERNAL_SURVEY;
            }
            else
            {
                surveyVoteInputFilterDto.TypeCreation = TYPE_PROCCESS_SURVEY.EXTERNAL_SURVEY;
            }
            await searchData();
        }


        public async Task searchData()
        {
            _spinnerService.Show();
            actualPageIndex = 1;
            await SearchData();
            _spinnerService.Hide();
        }

        private async Task SearchData()
        {
            surveyVoteInputFilterDto.SkipCount = (actualPageIndex - 1) * 10;
            surveyVoteInputFilterDto.MaxResultCount = 10;
          

            var responseListManagement = await _surveyService.GetSurveyExternalsFilter(surveyVoteInputFilterDto);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

            {
                // carga la data
               ModelDefinition = responseListManagement.definition;
                var celling = Math.Ceiling((decimal)responseListManagement.definition.totalCount / 10);
                totalAcountPages = (int)celling;
                StateHasChanged();
            }
            else
            {
                //error
                var message = responseListManagement != null && responseListManagement.response != null ?
                    responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
            }
        }

        public string getDateFormatDisplay(DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        public async Task OnPageClick(int pageIndex)
        {

            _spinnerService.Show();
            actualPageIndex = pageIndex;

            await SearchData();

            _spinnerService.Hide();
        }

        public async Task UpdateData(SurveyInputDto input)
        {
            _spinnerService.Show();
            actualPageIndex = 1;
            input.SkipCount = (actualPageIndex - 1) * 10;
            input.MaxResultCount = 10;
         

            var responseListManagement = await _surveyService.GetSurveyExternalsFilter(input);
            if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

            {
                // carga la data
                ModelDefinition = responseListManagement.definition;
                var celling = Math.Ceiling((decimal)responseListManagement.definition.totalCount / 10);
                totalAcountPages = (int)celling;
                StateHasChanged();
                _spinnerService.Hide();
            }
            else
            {
                //error
                var message = responseListManagement != null && responseListManagement.response != null ?
                    responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
                _spinnerService.Hide();
            }

            _spinnerService.Hide();
        }

        public void SurveyOrVote(SurveyResponseForVote item)
        {
            if(item.TypeSurvey == "SURVEY") {

                _navigation.NavigateTo($"/encuesta/{item.Id}");
            }
            else
            {
                _navigation.NavigateTo($"/votacion/{item.Id}");
            }
        }

        public void SurveyOrVoteStats(SurveyResponseForVote item)
        {
           

            _navigation.NavigateTo($"/opinion/resultados/{item.Id}");
           
        }


    }
}
