using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto
{
    #region principal item

    public class CatalogAutomaticResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Collection { get; set; }
        public string Description { get; set; }
        public string DisplayLabel { get; set; }
        public bool Enabled { get; set; }
    }


    public class CatalogAutomaticExcel
    {
        public string DisplayLabel { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }

    #endregion
    #region listado

    public class GetCatalogAutomaticResponse
    {
        public ResultModel response { get; set; }
        public CatalogAutomaticResponseDefinition definition { get; set; }
    }

    public partial class CatalogAutomaticResponseDefinition
    {
        public int totalCount { get; set; }
        public List<CatalogAutomaticResponseDto> items { get; set; }
    }


    public class GetCatalogResponseForListResponse
    {
        public ResultModel response { get; set; }
        public List<CatalogAutomaticResponseDto> definition { get; set; }
    }



    #endregion
    #region input de busqueda
    public class CatalogResponseInputDto : PagedResultRequestDto
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? DisplayLabel { get; set; }
        public bool? Enabled { get; set; }
    }


    public class CatalogResponseInputListDto
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? DisplayLabel { get; set; }
        public bool? Enabled { get; set; }
    }
    public class CatalogResponseInputListMultipleDto
    {
        public string[]? Codes { get; set; }
        public bool? Enabled { get; set; }
    }
    public class CatalogInputCollectionDto
    {
        public string[]? Collections { get; set; }
        public string? Ref1 { get; set; }
        public string? Ref2 { get; set; }
        public string? Ref3 { get; set; }
        public string? Ref4 { get; set; }
    }
    #endregion

    #region crud
    public class ManagementCatalogResponse
    {
        public Guid Id { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Code { get; set; }

        [StringLength(250)]
        public string Collection { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        [Required(ErrorMessage = "Campo requerido.")]
        public string Description { get; set; }

    
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(maximumLength: 250, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string DisplayLabel { get; set; }
        public bool Enabled { get; set; }
    }

    public class PostCatalogAutomaticResponse
    {
        public ResultModel response { get; set; }
        public object definition { get; set; }
    }
    #endregion
}
