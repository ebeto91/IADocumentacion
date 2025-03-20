using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers.CustomValidatorsForm;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class UserCredenditialsDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Password { get; set; }
    }

     public class RegeneratePasswordDto
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? EmailAddress { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        [EqualsResetCode]
        [CodeResetExpiration]
        public string? PasswordResetCode { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }

        public string? ResetCodeByUrl {  get; set; }
        public DateTime? PasswordExpiredDate { get; set; }
        public Guid? Id { get; set; }
    }


    public class RegisterCompleteDto
    {
       public string IdUser {  get; set; }
    }

    public class ChangePasswordResetModel
    {
        public string PasswordResetCode { get; set; }
        public DateTime? PasswordExpiredDate { get; set; }
        public Guid? Id { get; set; }
    }

    public class UserRegisterDto
    {

        [Required(ErrorMessage = "Tipo de Cédula obligatorio.")]
        public string? IdentificationTypeId { get; set; }

        [Required(ErrorMessage = "Cédula obligatoria.")]
        [DocumentValidator]
        //[DynamicMaxLength]
        public string? Identification { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio.")]
        [StringLength(maximumLength: 50, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Apellidos Obligatorios.")]
        [StringLength(maximumLength: 50, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Lastname { get; set; }


        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
       
        public string? Password { get; set; }

        public string? UserName { get; set; }

        public bool ? Enabled { get; set; }

        public string ? UserType { get; set; }
        [StringLength(maximumLength: 8, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(0, double.MaxValue, ErrorMessage = "No se permiten números negativos o letras")]
        public string? PhoneNumber { get; set;}

        [Required(ErrorMessage = "La confirmación es obligatoria.")]
        [EqualPassword]
        public string? PasswordConfirmar { get; set; }
    }
    public class UserInfoAccessDirective
    {
        public string Information { get; set; }
    }

    // Custom validation attribute for dynamic max length
    public class DynamicMaxLengthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var documentModel = validationContext.ObjectInstance as UserRegisterDto;
            if (documentModel != null)
            {
                int maxLength = documentModel.IdentificationTypeId == "3" ? 12 : 9;
                if (value != null && value.ToString().Length > maxLength)
                {
                    return new ValidationResult($"La cédula no puede tener más de {maxLength} caracteres.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
