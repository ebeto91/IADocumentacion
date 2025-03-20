using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey
{
    public class GetSurveyFilterResponse
    {
        public ResultModel response { get; set; }
        public List<SurveyResponseDataCarrousel> definition { get; set; }
    }

    public class SurveyResponseDataCarrousel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } // titulo
        public string? Description { get; set; } // problema a describir
        public SurveyPrincipalImageDto? PrincipalImage { get; set; } // imagen principal 
        public string? Status { get; set; } // estado si está aprobada o denegada
        public string TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        public string TypeSurvey { get; set; } // permite saber si es encuenta o votación
        public bool ShowResultsAlways { get; set; } // permite saber si se muestra al inicio o al final
        public bool AllowVoteOpen { get; set; } // votación abierta
        public DateTime StartDate { get; set; } //Inicio de aplicacion
        public DateTime DueRate { get; set; } //Fecha Finalización

    }

    public class SurveyPrincipalImageDto
    {

        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Guid SurveyId { get; set; }
        public bool ToDeleted { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
    }

    public class SurveyInputExternalDto
    {
        public DateTime? StartDate { get; set; } //Inicio de aplicacion
        public DateTime? DueRate { get; set; } //Fecha Finalización
    }



}
