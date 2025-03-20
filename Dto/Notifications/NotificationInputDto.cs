using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Notifications
{
    public class NotificationInputBellDto
    {
        public string? Id { get; set; }
        public string ToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? MarkAsRead { get; set; }
    }
    public class NotificationInputDto : PagedResultRequestDto
    {
        public string? Id { get; set; }
        public string? ToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? MarkAsRead { get; set; }
    }
}
