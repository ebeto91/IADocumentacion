using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management
{
    #region USER PROFILE MANAGMENT
    #region filter input 
    public class ManagementProfileInputFilterDto : PagedResultRequestDto
    {

        public string? Status { get; set; }
        public string? TypeApplication { get; set; }
        public string? PrincipalTypeApplication { get; set; }
        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        //public string? ComplainantName { get; set; }
        public string? ManagementEmail { get; set; }
        public string? PrincipalNumber { get; set; }
        public bool? IsFromProfile { get; set; }
        public string? Title { get; set; }

        public string? UserAssignedId { get; set; }

        public string? CreatedUserId { get; set; }

        public bool? ApplyToTask { get; set; } // saber si aplica una solicitud o no
        public string? Priority { get; set; } // Prioridad que va a tener
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? District { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio
        public string? ContactPoint { get; set; } //ContactPoint
        public string? AssociationRelatedMemoryId { get; set; }
    }

    #endregion
    #region principal model

    public class ManagementProfileDto
    {
        public string Id { get; set; }
        public string Description { get; set; } // lo que coloca en la descripcion el usuario
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        public string? District { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio
        public string? ContactPoint { get; set; } //ContactPoint
        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado
 
        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        public string Status { get; set; } // estado de la solicitud, para el aprobar no es necesario
        public string PrincipalNumber { get; set; } // numero de la solicitud
        public string? ResolutionNumber { get; set; } // numero de la resolución interna

        public string? OfficeCodeNumber { get; set; } // numero de oficio
        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
        public DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias
        public string? ManagementName { get; set; } //Nombre del solicitante

        public string? ManagementEmail { get; set; } //Correo solicitante

        public string? ManagementPhone { get; set; } //Telefono
        public string? ResolutionReason { get; set; } //ResolutionReason
        public string? Priority { get; set; } //Prioridad

        public string? ExternalManagementName { get; set; } //Nombre del solicitante

        public string? ExternalManagementEmail { get; set; } //Correo solicitante
        public string? ExternalManagementPhone { get; set; } //Telefono
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool ApplyToTask { get; set; } // saber si aplica una solicitud o no

        public Guid? WorkTaskAssignedBefore { get; set; } // me indica para mostrar si la solicitud ya tiene una tarea asignada
    }
    #endregion
    #region list response
    public class ResponseModelProfileManagementDto
    {
        public ResultModel response { get; set; }
        public ModelProfileManagementDefinition definition { get; set; }

    }
    public partial class ModelProfileManagementDefinition
    {
        public int totalCount { get; set; }
        public List<ManagementProfileDto> items { get; set; }
    }

    #endregion



    #endregion


    #region list and download excel

    #region response method 
    #region list response
    public class ResponseModelListManagementDto
    {
        public ResultModel response { get; set; }
        public ModelListManagementDefinition definition { get; set; }

    }

    public class ModelListManagementDefinition
    {
        public int totalCount { get; set; }
        public List<ManagementReportDto> items { get; set; }
    }

    #endregion

    #endregion
    public class ManagementReportDto
    {
        [Display(Name = "Title")]
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        [Display(Name = "Description")]
        public string Description { get; set; } // lo que coloca en la descripcion el usuario
        [Display(Name = "PrincipalNumber")]
        public string PrincipalNumber { get; set; } // numero de la solicitud
        [Display(Name = "StartDateApplication")]
        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        [Display(Name = "DueRateApplication")]
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
        [Display(Name = "FollowDateReminderToCreatorUser")]
        public DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias
        [Display(Name = "Status")]
        public string Status { get; set; } // estado de la solicitud, para el aprobar no es necesario
        [Display(Name = "Latitude")]
        public double? Latitude { get; set; } //latitud
        [Display(Name = "Longitude")]
        public double? Longitude { get; set; } //longitud
        [Display(Name = "District")]
        public string? District { get; set; } //distrito
        [Display(Name = "Neighborhood")]
        public string? Neighborhood { get; set; } //barrio
        [Display(Name = "ApplicableBudget")]
        public decimal? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar
        [Display(Name = "ContactPoint")]
        public string? ContactPoint { get; set; } //ContactPoint
        [Display(Name = "TypeApplication")]
        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado
        [Display(Name = "TypeCreation")]
        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        [Display(Name = "IsAnonymous")]
        public bool IsAnonymous { get; set; } // si es un usuario anonimo o no

        [Display(Name = "ResolutionNumber")]
        public string? ResolutionNumber { get; set; } // numero de la resolución interna
        [Display(Name = "OfficeCodeNumber")]
        public string? OfficeCodeNumber { get; set; } // numero de oficio
        [Display(Name = "ResolutionReason")]
        public string? ResolutionReason { get; set; } // numero de la resolución interna
        [Display(Name = "Priority")]
        public string? Priority { get; set; } // Prioridad que va a tener
        [Display(Name = "CreatedUserIpAddress")]
        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador
        [Display(Name = "ExternalManagementName")]
        public string? ExternalManagementName { get; set; } //Nombre del solicitante
        [Display(Name = "ExternalManagementEmail")]
        public string? ExternalManagementEmail { get; set; } //Correo solicitante
        [Display(Name = "ExternalManagementPhone")]
        public string? ExternalManagementPhone { get; set; } //Telefono
        [Display(Name = "Rating")]
        public double Rating { get; set; }
        //public SurveyQuestionOptionUserDto? SurveyQuestionOptionUser { get; set; }
    }

    #endregion
}
