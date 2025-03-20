using Microsoft.AspNetCore.Components;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVoteQA.Vote
{
    public class VoteQABase:ComponentBase
    {
        [Parameter]
        public string SurveyId { get; set; }
    }
}
