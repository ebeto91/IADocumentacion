using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department
{


    #region deparmentResponseUsers
    public class GetDepartmentUsersResponseGeneral
    {
        public ResultModel response { get; set; }
        public DepartmentResponseUsersDefinition definition { get; set; }
    }

    public class AssingUserDepartmentInputDto
    {
        [Required]
        public string DepartmentId { get; set; }
        [Required]
        public List<AssingUserDepartmentDto> ListAssingUsers { get; set; }
    }
    public class GetResponseAssignUsersGeneral
    {
        public ResultModel response { get; set; }
        public AssingUserDepartmentInputDto definition { get; set; }
    }

    public class DepartmentResponseUsersDefinition
    {
        public string? Description { get; set; } // descripcion departamento
        public string Name { get; set; }
        public string? Image { get; set; }
        public List<UserDepartmentDto> UserDepartments { get; set; }
    }

    public class UserDepartmentDto
    {
        public Guid Id { get; set; }
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Description { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Campo requerido.")]
        public string? Position { get; set; }
        public string? PositionDisplay { get; set; }
        public bool Enabled { get; set; }
        public string? Name { get; set; }

        public string? Lastname { get; set; }
        [StringLength(100)]
        public string? EmailAddress { get; set; }

        public Guid UserId { get; set; }

        public string? PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public string NameDepartment { get; set; }

        public bool Selected { get; set; }

    }

    public class InputsToApplyInFilterTable
    {
        public string? EmailAddress { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? DepartmentIdString { get; set; }
    }
    public class AssingUserDepartmentDto
    {
        public Guid UserId { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
        [StringLength(100)]
        public string? Position { get; set; }
        public bool Enabled { get; set; }
    }



    #endregion
}
