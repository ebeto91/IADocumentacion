using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey
{
    public class GetSurveyListAdminstratorResponseDefinition
    {
        public ResultModel response { get; set; }
        public GetSurveyListAdminstratorDefinition definition { get; set; }
    }

    public class GetSurveyListAdminstratorDefinition
    {
        public int totalCount { get; set; }
        public List<SurveyResponse> items { get; set; }
    }


    public class SurveyResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } // titulo
        public string? Description { get; set; } // problema a describir
        public SurveyPrincipalImageDto? PrincipalImage { get; set; } // imagen principal 
        public string? Status { get; set; } // estado si está aprobada o denegada
        public string TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        public string TypeSurvey { get; set; } // permite saber si es encuenta o votación
        public bool ShowResultsAlways { get; set; } // permite saber si se muestra al inicio o al final
        public bool AllowInformationEdit { get; set; } // permite saber si se encuentra con o sin votos para poder editar
        public bool AllowVoteOpen { get; set; } // votación abierta
        public DateTime StartDate { get; set; } //Inicio de aplicacion
        public DateTime DueRate { get; set; } //Fecha Finalización

        // Documentos para la encuesta
        public virtual List<SurveyAttachedDocumentResponse> SurveyAttachedDocuments { get; set; }
        public virtual List<SurveyQuestionResponse> SurveyQuestions { get; set; }

        public PartialAuditDto PartialInfoAudit { get; set; }
    }

    public class SurveyAttachedDocumentResponse
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public bool ToDeleted { get; set; }
    }


    public class SurveyQuestionResponse
    {
        public Guid? Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; } // titulo
        [StringLength(200)]
        public string? Note { get; set; } // nota adicional
        public bool IsRequired { get; set; } // indica si la pregunta es requerida
        [StringLength(50)]
        public string? Type { get; set; } // type en caso de necesitarse
        public bool AllowOtherValue { get; set; }
        public bool AllowJustification { get; set; } // si permite justificación

        [StringLength(maximumLength: 500, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? JustificationTitle { get; set; } // justificación titulo


        [StringLength(maximumLength: 500, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Justification { get; set; } // justificación escrita
        public int Order { get; set; }
        public bool ToDeleted { get; set; }
        public string ItemUniqueSelected { get; set; }

        public List<SurveyQuestionOptionResponse> SurveyQuestionOptions { get; set; }
    }
    public class SurveyQuestionOptionResponse
    {
        [Key]
        public Guid Id { get; set; }
        public string IdString { get; set; }
        [StringLength(100)]
        public string Description { get; set; } // Description
        public int Order { get; set; }

        public bool IsOtherValue { get; set; }
        public bool Checked { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? CommentValue { get; set; } // Description

        public SurveyQuestionOptionDocumentResponse? SurveyQuestionOptionDocuments { get; set; }

        public List<SurveyQuestionOptionUserResponse>? SurveyQuestionOptionUsers { get; set; }
        public bool ToDeleted { get; set; }

    }


    public class SurveyQuestionOptionDocumentResponse
    {
        [Key]
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public bool ToDeleted { get; set; }

    }

    public class SurveyQuestionOptionUserResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SurveyQuestionOptionId { get; set; }
        [StringLength(200)]
        public string? CommentValue { get; set; } // Description
    }

    #region input

    public class SurveyInputDto : PagedResultRequestDto
    {
        public string? Title { get; set; } // titulo
        [StringLength(50)]
        public string? Status { get; set; } // estado si está aprobada o denegada
        [StringLength(50)]
        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        [StringLength(50)]
        public string? TypeSurvey { get; set; } // permite saber si es encuenta o votación
        public DateTime? StartDate { get; set; } //Inicio de aplicacion
        public DateTime? DueRate { get; set; } //Fecha Finalización
    }

    #endregion

    #region surver response general
    public class GetSurveyItemResponseDefinition
    {
        public ResultModel response { get; set; }
        public SurveyResponse definition { get; set; }
    }

    #endregion


    public class RegisterVoteSurveyListDto
    {
        public Guid? UserId { get; set; }
        public Guid SurveyId { get; set; }
        public string? Ip { get; set; }
        public List<RegisterVoteSurvey> ListRegisterVote { get; set; }

    }
    public class RegisterVoteSurvey
    {

        public Guid SurveyQuestionId { get; set; }
        public List<Guid> SurveyQuestionOptionId { get; set; }
        public string? CommentValue { get; set; }
        public string? JustificationValue { get; set; }
    }


    public class TempAnswerTextOther
    {
         public Guid IdAnswer { get; set; }
        public string? TextAnswer { get; set; }

        public bool IsOther { get; set; }=true;
    }
}
