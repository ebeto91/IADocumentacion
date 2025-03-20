using Microsoft.AspNetCore.Components.Forms;
using NPOI.SS.Formula.Functions;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers.CustomValidatorsForm;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class ManagementUserDto
    {
        public string Id { get; set; }

        //[Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(maximumLength: 50, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Name { get; set; }

        //[Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(maximumLength: 50, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(maximumLength: 25, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Identification { get; set; }
        [DocumentValidator]
        [Required(ErrorMessage = "Campo Obligatorio")]

        public string IdentificationTypeId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Correo eléctronico no valido")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string EmailAddress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        //[Display(Name = "Nombre Usuario")]
        [StringLength(maximumLength: 8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "No se permiten números negativos o letras")]
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string PasswordResetCode { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool Enabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? AccessEndDateUntil { get; set; }
        public string UserType { get; set; }
        public DateTime? PwdExpiredDate { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }




    public class EditManagementUserDto
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Identification { get; set; }

        public string IdentificationTypeId { get; set; }

        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string UserName { get; set; }
        public bool SharedLocation { get; set; }

        public DateTime? LastLoginTime { get; set; }
        public bool Enabled { get; set; }
        public int AccessFailedCount { get; set; }
        public List<UserMethodNotificationDto>? UserMethodNotifications { get; set; }


        [Required]
        public int RoleId { get; set; }
    }

    public class UserMethodNotificationDto
    {
        public Guid? Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string Value { get; set; }

        public bool Selected { get; set; }
        public Guid UserId { get; set; }
    }

  

    public class UserProfileResponse : UserResponse
    {
        public List<UserMethodNotificationDto> UserMethodNotifications { get; set; } = new List<UserMethodNotificationDto>();
        public AssociationResponse? AssociationRelated { get; set; }
        public string RoleName { get; set; }
    }

    public class ResponseModelUserProfile
    {
        public ResultModel Response { get; set; }
        public UserProfileResponse Definition { get; set; }

    }

    public class UpdateImageUserProfile
    {
        public Guid UserId { get; set; }
        public IBrowserFile AttachedFile { get; set; }
    }

    public class UserAttachedPhotoInputDto
    {
        public Guid UserId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
    }

}
