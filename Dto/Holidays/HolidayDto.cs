using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays
{

    public class HolidaysListResponse
    {
        public ResultModel response { get; set; }
        public HolidaysListResponseDefinition definition { get; set; }
    }

    public partial class HolidaysListResponseDefinition
    {
        public int totalCount { get; set; }
        public List<HolidayDto> items { get; set; }
    }

    public class HolidaysGetAllListResponse
    {
        public ResultModel response { get; set; }
        public List<HolidayDto> definition { get; set; }
    }

    public class HolidaysGeneralResponse
    {
        public ResultModel response { get; set; }
        public HolidayDto definition { get; set; }
    }

    public class HolidayDto
    {
        public Guid? Id { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime DateSelected { get; set; }
    }

    public class HolidayManagementDto
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public DateTime? DateSelected { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Description { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }


    }

    public class HolidayDeleteDto
    {
        public Guid Id { get; set; }
    }

    public class HolidayExcel
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
