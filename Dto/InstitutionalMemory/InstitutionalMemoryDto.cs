using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory
{
    public class InstitutionalMemoryDto
    {
    }

    public class AssociationGroupByDistrictResponse
    {
        public ResultModel response { get; set; }
        public List<AssociationResponseGroupByDistrict> definition { get; set; }
    }


    public class NeighborhoodTemp
    {
        public string TextCustom { get; set; }
        public string ValueCustom { get; set; }

        public string Ref {  get; set; }
    }

    public class AssociationResponseGroupByDistrict
    {
        public string CodeDistrict { get; set; }
        public List<AssociationResponse> ListAssociations { get; set; }
    }



    public class AssociationResponse
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(300)]
        public string Name { get; set; }
        [StringLength(300)]
        public string? BeforeChangeName { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
        [StringLength(250)]
        public string? DistrictCode { get; set; }
        public string? DistrictLabel { get; set; }

        public string? NeighbordLabel { get; set; }
        [StringLength(250)]
        public string? NeighbordCode { get; set; }
        public UserResponse? UserRelated { get; set; }

    }
    public class InstitutionalMemoryResponse
    {
        public ResultModel response { get; set; }
        public object definition { get; set; }
    }






    public class ManagementDto
    {
        public Guid? Id { get; set; }
        [NotNull, StringLength(300)]
        public string Description { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(100)]
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        [StringLength(300)]
        public string? AditionalInformation { get; set; } // información adicional por si se requiere
        public double? Latitude { get; set; } //latitud
        public double? Longitude { get; set; } //longitud
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

        public bool AllowContact { get; set; } // si permite contactar
        public DateTime? DateIndicident { get; set; } //Fecha del incidente

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
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        //public SurveyQuestionOptionUserDto? SurveyQuestionOptionUser { get; set; }


        public Guid? WorkTaskAssignedBefore { get; set; } // me indica para mostrar si la solicitud ya tiene una tarea asignada
    }
}
