using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers.CustomValidatorsForm
{
    public class FormRegisterCedula
    {
    }

    public class DocumentValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Ensure context.ObjectInstance is of type UserRegisterDto
            if (validationContext.ObjectInstance is UserRegisterDto documentModel)
            {
                // Ensure value is not null or empty
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult("Cédula es Requerida", new[] { validationContext.MemberName });
                }

                string documentId = value.ToString();

                // Validate length based on IdentificationTypeId
                if (documentModel.IdentificationTypeId == TYPEIDENTIFICATION.National && documentId.Length != 9)
                {
                    return new ValidationResult("Cédula debe ser de 9 dígitos.", new[] { validationContext.MemberName });
                }

                if (documentModel.IdentificationTypeId == TYPEIDENTIFICATION.FOREIGN && documentId.Length != 12)
                {
                    return new ValidationResult("Pasaporte debe tener 12 dígitos", new[] { validationContext.MemberName });
                }

                if (documentModel.IdentificationTypeId == TYPEIDENTIFICATION.Legal && documentId.Length != 12)
                {
                    return new ValidationResult("Cédula Jurídica debe tener 12 dígitos", new[] { validationContext.MemberName });
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

    public class EqualPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Ensure context.ObjectInstance is of type UserRegisterDto
            if (validationContext.ObjectInstance is UserRegisterDto documentModel)
            {
               if(!string.IsNullOrWhiteSpace(documentModel.Password) && !string.IsNullOrEmpty(documentModel.PasswordConfirmar))
                {
                    if (documentModel.Password != documentModel.PasswordConfirmar)
                    {
                        return new ValidationResult("Los Campos Contaseña y Confirmar Contraseña deben de ser iguales.", new[] { validationContext.MemberName });
                    }
                }
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
