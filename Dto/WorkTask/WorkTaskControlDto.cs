using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask
{
    #region input
    public class WorkTaskInputDto : PagedResultRequestDto
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario


        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia u otra 

        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado

        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud

        public string? Status { get; set; } // estado de la solicitud

        public string? PrincipalNumber { get; set; } // numero de la solicitud

        public string? ResolutionNumber { get; set; } // numero de la resolución interna

        public string? OfficeCodeNumber { get; set; } // numero de oficio

        public string? UserId { get; set; }
        public string? UserAssignedId { get; set; }
        public string? Priority { get; set; } // Prioridad que va a tener
        public bool? IsVisible { get; set; } // visibiblidad

        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
    }
    #endregion
    #region response list for users internal



    public class GetWorkTaskForUsersAssignedResponse
    {
        public ResultModel response { get; set; }
        public GetWorkTaskForUsersAssignedDefinition definition { get; set; }
    }

    public class ResponseConvertSurveyToTask
    {
        public ResultModel response { get; set; }
        public AssingManagementTaskDto definition { get; set; }
    }

    public partial class GetWorkTaskForUsersAssignedDefinition
    {
        public int totalCount { get; set; }
        public List<WorkTaskResponse> items { get; set; }
    }

    public class WorkTaskResponse
    {
        public string Id { get; set; }
        public string Description { get; set; } // lo que coloca en la descripcion el usuario
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario

        public string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
        public string? District { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio

        public string? ContactPoint { get; set; } //ContactPoint
        public string? AditionalInformation { get; set; } //ContactPoint

        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia u otra 

        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado

        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        public string? TypeWorkTask { get; set; } // en caso de dividirlo en proyecto, obra menor

        public string Status { get; set; } // estado de la solicitud

        public string PrincipalNumber { get; set; } // numero de la solicitud

        public string? ResolutionNumber { get; set; } // numero de la resolución interna

        public string? OfficeCodeNumber { get; set; } // numero de oficio
        public bool IsVisible { get; set; } //  es visible la tarea para el kanban
        public string? ResolutionReason { get; set; } // numero de la resolución interna

        public string? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar

        public string? Priority { get; set; } // Prioridad que va a tener

        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador
        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización

        public DateTime CreatedAt { get; set; } //Fecha Finalización
        public DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias

        public string? ManagementName { get; set; } //Nombre del solicitante

        public string? ManagementEmail { get; set; } //Correo solicitante

        public string? ManagementPhone { get; set; } //Telefono

        public string? ExternalManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]

        public string? ExternalManagementEmail { get; set; } //Correo solicitante

        public string? ExternalManagementPhone { get; set; } //Telefono
        public double Rating { get; set; }

        public Guid? ManagementId { get; set; }

        public List<WorkTaskAttachedDocumentDto> AttachedDocuments { get; set; }
    }

    #region crud
    public class WorkTaskInputEditTrackableDto
    {

        public  Guid Id { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]

        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string Description { get; set; } // lo que coloca en la descripcion el usuario
        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? Title { get; set; } // lo que coloca en la descripcion el usuario

        public  string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
        public  string? District { get; set; } //distrito
        public  string? Neighborhood { get; set; } //barrio
        public DateTime? DateIndicident { get; set; }
        public string? NameUserIncident { get; set; }
        public bool AllowContact { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ContactPoint { get; set; } //ContactPoint
        [StringLength(50)]
        public  string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia u otra 
        public  string? AditionalInformation { get; set; } // tipo de la solicitud si es denuncia u otra 
        [StringLength(50)]
        //[Required(ErrorMessage = "Campo Obligatorio")]
        public  string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado
        [StringLength(50)]
        public  string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        [StringLength(50)]

        public  string? TypeWorkTask { get; set; } // en caso de dividirlo en proyecto, obra menor
      //  [Required(ErrorMessage = "Campo Obligatorio")]
        public  string? TypeWorkTaskCustom { get; set; } // en caso de dividirlo en proyecto, obra menor
        [StringLength(50)]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public  string Status { get; set; } // estado de la solicitud
        [StringLength(100)]
        public  string PrincipalNumber { get; set; } // numero de la solicitud

        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ResolutionNumber { get; set; } // numero de la resolución interna
        [StringLength(100)]
        public  string? OfficeCodeNumber { get; set; } // numero de oficio
        public  bool IsVisible { get; set; } //  es visible la tarea para el kan    ban

        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ResolutionReason { get; set; } // numero de la resolución interna

        [StringLength(maximumLength: 16, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "No se permiten números negativos o letras")]
        public  string? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar
        [StringLength(50)]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public  string? Priority { get; set; } // Prioridad que va a tener
        [StringLength(20)]
        public  string? CreatedUserIpAddress { get; set; } // IP del usuario creador
        public  DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public  DateTime? DueRateApplication { get; set; } //Fecha Finalización
        public  DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias
        // New properties for the Management's details
        [StringLength(100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ManagementEmail { get; set; } //Correo solicitante
        [StringLength(8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ManagementPhone { get; set; } //Telefono
        [StringLength(100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ExternalManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ExternalManagementEmail { get; set; } //Correo solicitante
        [StringLength(8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public  string? ExternalManagementPhone { get; set; } //Telefono
        public  double Rating { get; set; }

        public Guid? ManagementId { get; set; }


        public string? AssociationMemory {  get; set; }

        public string? AssociationRelatedMemoryId { get; set; }

        public AssociationResponse? AssociationRelatedMemory { get; set; }
        public List<WorkTaskAttachedDocumentDto>? AttachedDocuments { get; set; }

        public List<IBrowserFile>? AttachedNewFiles { get; set; }
    }

    public class WorkTaskInputCreateDto : WorkTaskInputEditTrackableDto
    {
        public DateTime? CreatedAt { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string? DistrictCustom { get; set; } //distrito
    }
    public class WorkTaskAttachedDocumentDto
    {
        public Guid Id { get; set; }
        public Guid WorkTaskId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public bool ToDeleted { get; set; }
        public IBrowserFile? NewFile { get; set; }
    }

    #endregion
    #endregion
    #region input filter worktask

    #endregion

    #region get definition
    public class GetWorkTaskByIdResponseDefinition
    {
        public ResultModel response { get; set; }
        public WorkTaskResponseDetail definition { get; set; }
    }


    public class WorkTaskResponseDetail : WorkTaskResponse
    {
        public List<WorkTaskUsersAssignedDto> ListUsersSelectable { get; set; }
        public List<WorkTaskUsersAssignedDto> ListAssignedUsers { get; set; }
        public PartialAuditDto PartialInfoAudit { get; set; }
        public AssociationResponse? AssociationRelatedMemory { get; set; }
    }

    public class WorkTaskUsersAssignedDto
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }

        public string WorkTaskId { get; set; }

        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? LastNotificationTime { get; set; }
        public bool Enabled { get; set; }
        public bool Selected { get; set; }
        public bool IsUserBeforeAssigned { get; set; } // se utiliza para el tema de obtener los resultados y  saber quien está seleccionado o no
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Lastname { get; set; }
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [StringLength(8)]
        public string? PhoneNumber { get; set; }
        public string? Position { get; set; }

        [StringLength(300)]
        public string? ReasonChangeStatus { get; set; }
        [StringLength(300)]
        public string UserPositionTask { get; set; } //Por si se requiere saber que usuario es el encargado en la tarea
    }

    #region partial audit
    public class PartialAuditDto
    {
        //public bool IsDeleted { get; set; }
        //public Guid? DeletedUserId { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedUserName { get; set; }
        public string? CreatedEmailAddress { get; set; }

        public Guid? UpdatedUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedUserName { get; set; }
        public string? UpdatedEmailAddress { get; set; }


    }
    #endregion

    #endregion

    #region HandleChangeUsersAssingWorkTask
    public class HandleChangeUsersAssingWorkTaskResponse
    {
        public ResultModel? response { get; set;  }
        public object? definition { get; set;  }

    }

    public class HandleChangeUsersAssingWorkTask
    {
        public string WorkTaskId { get; set; }
        public List<HandleChangeUsersDepartmentAssingWorkTask> DepartmentList { get; set; }

    }

    #region worktask department users
    public class HandleChangeUsersDepartmentAssingWorkTask
    {
        public string DepartmentId { get; set; }
        public List<WorkTaskUsersAssignedDto> ListAssingUsers { get; set; }
    }
    #endregion

    #endregion

    public class HandleRaiseMemoryToWorktask
    {
        public string? ManagementId { get; set; }
    }
}
