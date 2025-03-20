using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto
{
    public class CatalogResponse
    {
        public ResultModel response { get; set; }
        public List<Catalog> definition { get; set; }
    }

    public partial class CatalogResponseDefinition
    { 
        public List<Catalog> items { get; set; }
    }
}
