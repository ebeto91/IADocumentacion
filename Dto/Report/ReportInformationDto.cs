using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Report
{
    public class GetReportInformationDto
    {
        public ResultModel response { get; set; }
        public string definition { get; set; }
    }

    public class ReportInformationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmbedUrl { get; set; }
        public string Token { get; set; }
    }
}
