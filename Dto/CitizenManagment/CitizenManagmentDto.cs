using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment
{
    public class CitizenManagmentDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public string Details { get; set; }

        public bool Info { get; set; }
        public IBrowserFile FileImage { get; set; }

        public string Identification { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentificationP { get; set; }

        public string NameP { get; set; }

        public string EmailP { get; set; }

        public string PhoneNumberP { get; set; }



    }



    public class DenunciaFuncionario
    {
      [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(300)]
        public string Description { get; set; }
      
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        
        public string? Status { get; set; } // estado de la memoria
        public string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
        [Required(ErrorMessage = "Campo Obligatorio.")]
        public DateTime? DateIndicident { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? NameUserIncident { get; set; }

        public bool AllowContact { get; set; }
        public string LatitudeS { get; set; } //latitud
        public string LongitudeS { get; set; } //longitud
        public string? District { get; set; } //distrito

        [Required(ErrorMessage = "Campo Obligatorio.")]
        public string? DistrictCustom { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio
        public Guid? AssociationRelatedMemoryId { get; set; } //asocición relacionada
       

        public string? ContactPoint { get; set; } //ContactPoint

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ContactPointCustom { get; set; } //ContactPoint


        public string PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        public string? TypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        public bool IsAnonymous { get; set; }
        //public string ManagementType { get; set; }
        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador

        // New properties for the Management's details
        public string? ManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        public string? ManagementEmail { get; set; } //Correo solicitante
        public string? ManagementPhone { get; set; } //Telefono

        public string? ExternalManagementName { get; set; } //Nombre del solicitante
        
        public string? ExternalManagementEmail { get; set; } //Correo solicitante
       
        public string? ExternalManagementPhone { get; set; } //Telefono

        public List<IBrowserFile> attachedFiles { get; set; }
    }









    public class CreateManagementInputDto
    {
        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(300)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MaxLength(100)]
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario
        
        public string? Status { get; set; } // estado de la memoria
        public string? Latitude { get; set; } //latitud
        public string? Longitude { get; set; } //longitud
   
        public DateTime? DateIndicident { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? NameUserIncident { get; set; }
        public bool AllowContact { get; set; }
        public string LatitudeS { get; set; } //latitud
        public string LongitudeS { get; set; } //longitud
        public string? District { get; set; } //distrito

      
        public string? DistrictCustom { get; set; } //distrito
        public string? Neighborhood { get; set; } //barrio
        public Guid? AssociationRelatedMemoryId { get; set; } //asocición relacionada
       

        public string? ContactPoint { get; set; } //ContactPoint


        public string? ContactPointCustom { get; set; } //ContactPoint


        public string PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        public string? TypeApplication { get; set; } // tipo de la solicitud si es denuncia o gestion o memoria
        public bool IsAnonymous { get; set; }
        //public string ManagementType { get; set; }
        public string? CreatedUserIpAddress { get; set; } // IP del usuario creador

        // New properties for the Management's details
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ManagementName { get; set; } //Nombre del solicitante
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ManagementEmail { get; set; } //Correo solicitante
        [StringLength(maximumLength: 8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ManagementPhone { get; set; } //Telefono
        public string? TypeCreation { get; set; }
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ExternalManagementName { get; set; } //Nombre del solicitante
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ExternalManagementEmail { get; set; } //Correo solicitante
        [StringLength(maximumLength: 8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? ExternalManagementPhone { get; set; } //Telefono

        public List<IBrowserFile> attachedFiles { get; set; }

        public List<EvidenciaDto>? streamContent { get; set; }

        public DateTime? FollowDateReminderToCreatorUser { get; set; } = DateTime.Today; //Fecha de seguimiento para las memorias

        [StringLength(maximumLength: 16, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "No se permiten números negativos o letras")]
        public string? ApplicableBudget { get; set; } // monto del presuesto en caso de aplicar

        public string? Priority { get; set; } // Prioridad que va a tener

        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización

        public DateTime CreationDate { get; set; } //Fecha Finalización

        public List<WorkTaskUsersAssignedInputDto>? UserDepartmentList { get; set; }
    }

    public class EvidenciaDto
    {
        public StreamContent? streamContentImage {  get; set; }
        public string imageUrl { get; set; }
        public string Identificador { get; set; }

        public bool Delete { get; set; }
        public IBrowserFile FileImage { get; set; }
    }

    public class GetManagmentCitizenFilterResponse
    {
        public ResultModel response { get; set; }
        public object definition { get; set; }
    }



}
