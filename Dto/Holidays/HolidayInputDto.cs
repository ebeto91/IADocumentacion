using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays
{
    public class HolidayInputDto : PagedResultRequestDto
    {

        public DateTime? DateSelected { get; set; }
        public string Description { get; set; }
    }

    public class HolidayInputAllDto
    {

        public DateTime? DateSelected { get; set; }

        public bool? GetFutureDays { get; set; }
    }
}
