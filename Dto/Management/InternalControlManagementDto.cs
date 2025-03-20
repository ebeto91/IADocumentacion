using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management
{
    #region detail management
    #region definition
    public class GetManagementInfoResponse
    {
        public ResultModel response { get; set; }
        public FullManagementDto definition { get; set; }
    }
    #endregion
    public class FindManagementByPrincipalNumberDto
    {
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string PrincipalNumberToFind { get; set; }
    }
    public class FullManagementDto
    {
        public Guid Id { get; set; }
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? AditionalInformation { get; set; } // información adicional por si se requiere
        public string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
        public string? Identification { get; set; } //Identification
        public string? District { get; set; } //distrito

        public string? Neighborhood { get; set; } //barrio
        [StringLength(maximumLength: 16, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "No se permiten números negativos o letras")]
        public string? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar
        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ContactPoint { get; set; } //ContactPoint
        [Required(ErrorMessage = "Campo requerido.")]
        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        [Required(ErrorMessage = "Campo requerido.")]
        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado

        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        public bool IsAnonymous { get; set; } // si es un usuario anonimo o no
        [Required(ErrorMessage = "Campo requerido.")]
        public string Status { get; set; } // estado de la solicitud

        public string PrincipalNumber { get; set; } // numero de la solicitud
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ResolutionNumber { get; set; } // numero de la resolución interna
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]

        public string? OfficeCodeNumber { get; set; } // numero de oficio
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ResolutionReason { get; set; } // numero de la resolución interna
        public bool ApplyToTask { get; set; } // saber si aplica una solicitud o no
        [Required(ErrorMessage = "Campo requerido.")]
        public string? Priority { get; set; } // Prioridad que va a tener

        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador
        public int NumRetriesApplication { get; set; } // Numero de reintentos
        public bool AllowContact { get; set; } // si es un usuario anonimo o no

        public DateTime? DateIndicident { get; set; } //Fecha del incidente
        public string? NameUserIncident { get; set; }


        public DateTime? DateRetry { get; set; } // Numero de reintentos
   
        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion

        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
        public DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias
        [StringLength(100)]
        public string? ManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ManagementEmail { get; set; } //Correo solicitante

        public string? ManagementPhone { get; set; } //Telefono
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ExternalManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ExternalManagementEmail { get; set; } //Correo solicitante
        [StringLength(maximumLength: 8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ExternalManagementPhone { get; set; } //Telefono
        public double Rating { get; set; }
        public AssociationResponse? AssociationRelatedMemory { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? UpdatedUserId { get; set; }


        //public List<ManagementHistoryDto> ManagementHistories { get; set; }

        public List<ManagementAttachedDocumentDto> AttachedDocuments { get; set; }
    }
    public class ManagementAttachedDocumentDto
    {
        public Guid Id { get; set; }
        public Guid ManagementId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public string TypeApplication { get; set; }

    }

    #endregion

    #region complaint full
    public class FullComplaitDto : FullManagementDto
    {
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ContactPoint { get; set; } //ContactPoint
       
    }
    #endregion


    #region crud
    public class ManagementDto
    {
        public Guid Id { get; set; }
        [NotNull, StringLength(300)]
        public string Description { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(100)]
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(300)]
        public string? AditionalInformation { get; set; } // información adicional por si se requiere
        public string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
        public string? District { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio
        public decimal? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar
        [StringLength(200)]
        public string? ContactPoint { get; set; } //ContactPoint
        [StringLength(50)]
        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        [StringLength(50)]
        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado
        [StringLength(50)]
        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud
        public bool IsAnonymous { get; set; } // si es un usuario anonimo o no
        [StringLength(50)]
        public string Status { get; set; } // estado de la solicitud, para el aprobar no es necesario
        [StringLength(100)]
        public string PrincipalNumber { get; set; } // numero de la solicitud
        [StringLength(100)]
        public string? ResolutionNumber { get; set; } // numero de la resolución interna
        [StringLength(100)]
        public string? OfficeCodeNumber { get; set; } // numero de oficio
        [StringLength(300)]
        public string? ResolutionReason { get; set; } // numero de la resolución interna
        public bool ApplyToTask { get; set; } // saber si aplica una solicitud o no
        [StringLength(50)]
        public string? Priority { get; set; } // Prioridad que va a tener
        [StringLength(20)]
        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador
        public int NumRetriesApplication { get; set; } // Numero de reintentos
        public DateTime? DateRetry { get; set; } // Numero de reintentos
        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización

        public DateTime? FollowDateReminderToCreatorUser { get; set; } //Fecha de seguimiento para las memorias
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

        public DateTime? DateIndicident { get; set; }
        public string? NameUserIncident { get; set; }
        public bool AllowContact { get; set; }

        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        //public SurveyQuestionOptionUserDto? SurveyQuestionOptionUser { get; set; }
    }
    #endregion
}
