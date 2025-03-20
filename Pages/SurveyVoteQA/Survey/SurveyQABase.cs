using Microsoft.AspNetCore.Components;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVoteQA.Survey
{
    public class SurveyQABase:ComponentBase
    {
        [Parameter]
        public string SurveyId { get; set; }
    }
}
