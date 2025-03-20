using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department
{
    #region principalItems

    public class DepartmentResponse
    {
        public Guid Id { get; set; }
        public string? Description { get; set; } // descripcion departamento
        public string Name { get; set; }
        public string CodeDepartment { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmailLeader { get; set; }
        public string? PhoneLeader { get; set; }
        public string? UserNameLeader { get; set; }
        public string? IdentificationLeader { get; set; }
        public string? IdLeader { get; set; }
        public bool IsEditable { get; set; }
        public string? Image { get; set; }
    }
    #endregion

    #region listado
    public class GetDepartmentListResponse
    {
        public ResultModel response { get; set; }
        public DepartmentListResponseDefinition definition { get; set; }
    }

    public partial class DepartmentListResponseDefinition
    {
        public int totalCount { get; set; }
        public List<DepartmentResponse> items { get; set; }
    }


    #endregion

    #region responses
    public class GetDepartmentGeneralResponse
    {
        public ResultModel response { get; set; }
        public DepartmentResponse definition { get; set; }
    }


    #endregion
    #region input
    public class DepartmentInputDto : PagedResultRequestDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class DepartmentIdInputDto
    {
        public string Id { get; set; }
    }
    #endregion

    #region crud
    public class ManagementDepartmentDto
    {
        public string? Id { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        //[Required(ErrorMessage = "Campo requerido.")]
        public string? Description { get; set; } // descripcion departamento
        public string? BeforeChangeName { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? CodeDepartment { get; set; }
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Correo eléctronico no valido")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? EmailAddress { get; set; }
        public bool? IsEditable { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Name { get; set; }
        public string? Image { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string? IdUserLeadershipDepartment { get; set; }
        public string? IdUserLeadershipDepartmentBefore { get; set; }
    }


    public class DepartmenstExcelDto
    {
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Name { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        //[Required(ErrorMessage = "Campo requerido.")]
        public string? Description { get; set; } // descripcion departamento
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? CodeDepartment { get; set; }
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Correo eléctronico no valido")]
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? EmailAddress { get; set; }
        public string? Image { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string? IdUserLeadershipDepartment { get; set; }
    }


    #endregion




}
