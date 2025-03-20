using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class GetAllUserResponseDto
    {
        public ResultModel response { get; set; }
        public List<GetUserForDropDownList> definition { get; set; }
    }

    public class GetUserForDropDownList
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Identification { get; set; }


        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public string UserName { get; set; }
    }

    public class UserIdentificationInputDto
    {
        public string? Identification { get; set; }
    }
}
