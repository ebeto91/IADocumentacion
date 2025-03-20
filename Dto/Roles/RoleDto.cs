using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles
{
    #region basic
    public class RoleDto
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Name { get; set; }
        [StringLength(maximumLength: 100, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Description { get; set; }
        public bool Enabled { get; set; }

        public string FilterRole { get; set; }
    }
    #endregion
    #region response consult create / edit

    public class GetRolesResponse
    {
        public ResultModel response { get; set; }
        public List<RoleDto> definition { get; set; }
    }



    #endregion
    #region input 
    public class RoleFilterRequest : PagedResultRequestDto
    {
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public string? FilterRole { get; set; }
    }

    public class RoleFilter
    {
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public string? FilterRole { get; set; }
        public int? IdRole { get; set; }
    }

    public class RoleFilterRequestInput
    {
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public string? FilterRole { get; set; }
    }
    #endregion
    #region listado
    public class GetRolListResponse
    {
        public ResultModel response { get; set; }
        public RolListResponseDefinition definition { get; set; }
    }

    public partial class RolListResponseDefinition
    {
        public int totalCount { get; set; }
        public List<RoleDto> items { get; set; }
    }

    public class GetRolProcessResponse
    {
        public ResultModel response { get; set; }
        public object definition { get; set; }
    }

    #endregion
}
