using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey
{
    public class SurveyQuestionDto
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Title { get; set; } // titulo
        [StringLength(maximumLength: 200, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Note { get; set; } // nota adicional
        public bool IsRequired { get; set; } // indica si la pregunta es requerida
        [StringLength(50)]
        public string? Type { get; set; } // type en caso de necesitarse
        public bool AllowOtherValue { get; set; }
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string DescriptionOtherValue { get; set; }

        public bool AllowJustification { get; set; } // si permite justificación

        [StringLength(maximumLength: 500, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? JustificationTitle { get; set; } // justificación titulo

        [StringLength(maximumLength: 500, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Justification { get; set; } // justificación escrita

        public int Order { get; set; }
        public string OrderString { get; set; }
        public bool ToDeleted { get; set; }
        public bool ToAddEdit { get; set; }


        public List<SurveyQuestionOptionDto> SurveyQuestionOptions { get; set; }

        #region for front
        public bool IsForEditable { get; set; }
        public bool IsDefault { get; set; }

        #endregion
    }

    public class SurveyQuestionOptionDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Description { get; set; } // Description
        public int Order { get; set; }
        public string OrderString { get; set; }
        public bool IsOtherValue { get; set; }
        public SurveyQuestionOptionDocumentDto? SurveyQuestionOptionDocuments { get; set; }
        public IBrowserFile? NewFileQuestion { get; set; }
        public string? Url { get; set; }

        public List<SurveyQuestionOptionUserDto>? SurveyQuestionOptionUsers { get; set; }
        public bool ToDeleted { get; set; }
        public bool ToAddEdit { get; set; }
    }


    public class SurveyQuestionOptionDocumentDto
    {
        [Key]
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public bool ToDeleted { get; set; }

    }

    public class SurveyQuestionOptionUserDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid SurveyQuestionOptionId { get; set; }
        [StringLength(200)]
        public string? CommentValue { get; set; } // Description
    }
}
