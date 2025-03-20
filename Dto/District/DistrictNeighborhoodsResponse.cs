using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District
{
    public class DistrictNeighborhoodsDefinition
    {
        public string Code { get; set; }
        public string Collection { get; set; }
        public string Description { get; set; }
        public string DisplayLabel { get; set; }
        public string Ref1 { get; set; }

        public List<NeighborhoodDto> NeighborhoodList { get; set; }
    }

    public class NeighborhoodDto : Catalog
    {

    }


    public class DistrictNeighborhoodsResponse
    {
        public ResultModel response { get; set; }
        public List<DistrictNeighborhoodsDefinition> definition { get; set; }
    }
}
