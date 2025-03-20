using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote
{
    public class ResponseModelModelSurveyVote
    {
        public ResultModel response { get; set; }
        public ModelSurveyVoteDefinition definition { get; set; }

    }

    public class ResponseModelQuestionSurveyVote
    {
        public ResultModel response { get; set; }
        public SurveyResponse definition { get; set; }

    }

    public class ResponseModel
    {
        public ResultModel response { get; set; }
        public object definition { get; set; }

    }


    public class HangleManagementTaskBySurvey : HandleAssingWorkTask
    {
        public Guid SurveyQuestionOptionUserId { get; set; }
    }

    public class HandleAssingWorkTask : ManagementWorkTask
    {
        public List<WorkTaskUsersAssignedInputDto> UserDepartmentList { get; set; }
    }

    public class ManagementWorkTask
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string Description { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(100)]
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(300)]
        public string? AditionalInformation { get; set; } // información adicional por si se requiere
        public string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
        public string? District { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio
        [StringLength(200)]
        public string? ContactPoint { get; set; } //ContactPoint
        [StringLength(50)]
        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia u otra 
        [StringLength(50)]
        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado
        [StringLength(50)]
        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        [StringLength(50)]
        public string? TypeWorkTask { get; set; } // en caso de dividirlo en proyecto, obra menor
        [StringLength(50)]
        public string Status { get; set; } // estado de la solicitud
        [StringLength(100)]
        public string? PrincipalNumber { get; set; } // numero de la solicitud
        [StringLength(100)]
        public string? ResolutionNumber { get; set; } // numero de la resolución interna
        [StringLength(100)]
        public string? OfficeCodeNumber { get; set; } // numero de oficio

        public bool IsVisible { get; set; } //  es visible la tarea para el kanban
        [StringLength(300)]
        public string? ResolutionReason { get; set; } // numero de la resolución interna
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar
        [StringLength(50)]
        public string? Priority { get; set; } // Prioridad que va a tener
        [StringLength(20)]
        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador
        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
        public DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias
        // New properties for the Management's details
        [StringLength(100)]
        public string? ManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string? ManagementEmail { get; set; } //Correo solicitante
        [StringLength(8)]
        public string? ManagementPhone { get; set; } //Telefono
        [StringLength(100)]
        public string? ExternalManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string? ExternalManagementEmail { get; set; } //Correo solicitante
        [StringLength(8)]
        public string? ExternalManagementPhone { get; set; } //Telefono
        public double? Rating { get; set; }

    }

    public partial class ModelSurveyVoteDefinition
    {
        public int totalCount { get; set; }
        public List<SurveyResponseForVote> items { get; set; }
    }


    public class SurveyResponseForVote : SurveyResponse
    {
        public bool IsVoteRegister { get; set; } // votación abierta

    }

}
