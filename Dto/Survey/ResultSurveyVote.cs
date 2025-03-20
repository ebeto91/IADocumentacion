using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey
{
    public class ResultSurveyVote
    {
        public ResultModel response { get; set; }
        public ShowResultsResponse definition {  get; set; }
    }

    public class ShowResultsInputDto
    {
        public Guid SurveyId { get; set; }
    }

    public class ShowResultsResponse
    {
        public Guid Id { get; set; }

        public virtual List<SurveyQuestionShowResult> SurveyQuestions { get; set; }
    }

    public class SurveyQuestionShowResult
    {
        public Guid? Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; } // titulo
        [StringLength(200)]
        public string? Note { get; set; } // nota adicional
        public int Order { get; set; }
        public bool AllowOtherValue { get; set; }
        public int TotalVotesQuestions { get; set; }
        public string? Type { get; set; }


        public List<SurveyQuestionOptionShowResult> SurveyQuestionOptions { get; set; }
    }

    public class SurveyQuestionOptionShowResult
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Description { get; set; } // Description
        public int Order { get; set; }
        public bool IsOtherValue { get; set; }
        public int Total { get; set; }
        public double TotalPercentage { get; set; }

        public SurveyQuestionOptionDocumentResponse? SurveyQuestionOptionDocumentResponse { get; set; } // imagen
        public List<QuestionComment>? QuestionOptionsComments { get; set; }

    }

  

    public class QuestionComment
    {
        public string? CommentValue { get; set; }
        public Guid? SurveyQuestionOptionUserId { get; set; }
        public bool AllowForTask { get; set; }

        public bool Hide { get; set; } = false;

        public string  Title { get; set; }

        public string Descripcion { get; set; }
    }
}
