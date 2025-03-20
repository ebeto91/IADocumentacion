using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management
{
    public class RatingManagementDto
    {
        public double Rating { get; set; }
        public string ManagementId { get; set; }
    }


    public class RatingManagementResponse
    {
        public ResultModel response { get; set; }
        public bool definition { get; set; }
    }
}
