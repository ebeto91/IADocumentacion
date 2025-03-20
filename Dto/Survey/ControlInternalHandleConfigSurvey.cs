using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey
{


    #region edit/ create HandleSurveyConfig
    public class HandleSurveyConfig
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Title { get; set; } // titulo
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Description { get; set; } // problema a describir

        public SurveyPrincipalImageDto? PrincipalImage { get; set; } // imagen principal 
        public IBrowserFile? NewPrincipalImage { get; set; } // imagen principal 
        public string? UrlPrincipal { get; set; } // imagen principal 

        public string? Status { get; set; } // estado si está aprobada o denegada
        public string TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        [Required(ErrorMessage = "Campo requerido.")]
        public string TypeSurvey { get; set; } // permite saber si es encuenta o votación
        public bool ShowResultsAlways { get; set; } // permite saber si se muestra al inicio o al final
        public bool AllowVoteOpen { get; set; } // votación abierta
        public DateTime StartDate { get; set; } //Inicio de aplicacion
        public DateTime DueRate { get; set; } //Fecha Finalización

        // Documentos para la encuesta
        public virtual List<SurveyAttachedDocumentDto>? SurveyAttachedDocuments { get; set; }

        public virtual List<IBrowserFile>? AttachedNewFiles { get; set; }
        public virtual List<SurveyQuestionDto> SurveyQuestions { get; set; }
    }
    public class SurveyAttachedDocumentDto
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public bool ToDeleted { get; set; }
        public IBrowserFile BrowserFile { get; set;}
    }

    //public class SurveyPrincipalImageDto
    //{

    //    public Guid Id { get; set; }
    //    public string FilePath { get; set; }
    //    public string FileName { get; set; }
    //    public Guid SurveyId { get; set; }
    //    public bool ToDeleted { get; set; }
    //    [StringLength(100)]
    //    public string MimeType { get; set; }
    //}
    #endregion


    #region CHANGE STATUS
    public class ChangeStatusSurveyDto
    {
        public Guid? Id { get; set; }
        public string? Status { get; set; } // estado si está aprobada o denegada
        public string ReasonDenied { get; set; } // en caso de denegarse manda motivo
    }
    #endregion
}
