using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers.CustomValidatorsForm
{
    public class ResetPassword
    {
    }

    public class EqualsResetCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Ensure context.ObjectInstance is of type UserRegisterDto
            if (validationContext.ObjectInstance is ChangePasswordDto documentModel)
            {
                if (!string.IsNullOrEmpty(documentModel.PasswordResetCode))
                {
                    if(documentModel.PasswordResetCode != documentModel.ResetCodeByUrl)
                    {
                        return new ValidationResult("Código tiene que ser igual al enviado al correo electrónico", new[] { validationContext.MemberName });
                    }
                }

                // Return success if all validations pass
                return ValidationResult.Success;
            }

            else
            {
                // Return success if context.ObjectInstance is not of type UserRegisterDto
                return ValidationResult.Success;
            }
        }
    }
    public class CodeResetExpiration : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Ensure context.ObjectInstance is of type UserRegisterDto
            if (validationContext.ObjectInstance is ChangePasswordDto documentModel)
            {
                if (!string.IsNullOrEmpty(documentModel.PasswordResetCode))
                {
                    if (documentModel.PasswordResetCode == documentModel.ResetCodeByUrl)
                    {
                        if (documentModel.PasswordExpiredDate < DateTime.Now)
                        {
                            return new ValidationResult("El código ya expiró, debe solicitar un nuevo código.", new[] { validationContext.MemberName });
                        }
                    }
                }

                // Return success if all validations pass
                return ValidationResult.Success;
            }

            else
            {
                // Return success if context.ObjectInstance is not of type UserRegisterDto
                return ValidationResult.Success;
            }
        }
    }

}
