using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict
{
    #region principal item

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
        public string DistrictCode { get; set; }
        public string DistrictLabel { get; set; }
        public string? NeighbordCode { get; set; }
        public string? NeighbordLabel { get; set; }
        public UserResponse UserRelated { get; set; }

    }
    #region user associaction
    public class GetAssociationListUsersResponse
    {
        public ResultModel response { get; set; }
        public AssociationListUsersResponse definition { get; set; }
    }

    public class AssociationListUsersResponse
    {
        public UserResponse? UserRelated { get; set; }

        public IEnumerable<UserResponse> ListUsersToJoin { get; set; }

    }

    #endregion



    #endregion
    #region listado

    public class GetAssociationResponse
    {
        public ResultModel response { get; set; }
        public AssociationDefinition definition { get; set; }
    }

    public partial class AssociationDefinition
    {
        public int totalCount { get; set; }
        public List<AssociationResponse> items { get; set; }
    }

    #endregion
    #region input de busqueda
    public class AssociationInputDto : PagedResultRequestDto
    {
        public string? Name { get; set; }
        public string? DistrictCode { get; set; }
    }


    public class AssociationIdInputDto
    {
        public Guid Id { get; set; }
    }
    #endregion

    #region crud
    public class HandleAssociationConfigExcel
    {
        [Required(ErrorMessage = "Campo requerido.")]

        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(250)]
        public string DistrictCode { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(250)]
        public string NeighbordCode { get; set; }
    }
    public class HandleAssociationConfig
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
 
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Name { get; set; }
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? BeforeChangeName { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 300, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(250)]
        public string DistrictCode { get; set; }

        [StringLength(250)]
        public string? NeighbordCode { get; set; }
    }
    public class HandleAssociationJoinUser
    {

        public Guid AssociationId { get; set; }
        public string UserId { get; set; }


    }


    public class PostAssociationProcessResponse
    {
        public ResultModel response { get; set; }
        public object definition { get; set; }
    }
    #endregion
}
