using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class UserInputDto : PagedResultRequestDto
    {
        public string? Id { get; set; }
        public string? EmailAddress { get; set; }
        public string? UserName { get; set; }
        public string? BadgeCode { get; set; }
        public string? Email { get; set; }
        public string? Identification { get; set; }
        public string? IdentificationTypeId { get; set; }
        public string? UserType { get; set; }
        public string? AssignedUserFilter { get; set; }
        public bool? Enabled { get; set; }
    }
    public class UserInputControlDto
    {
        public string Id { get; set; }
    }
}
