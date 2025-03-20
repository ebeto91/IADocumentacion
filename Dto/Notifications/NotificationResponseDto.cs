using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Notifications
{
    public class NotificationResponseDto
    {
        public ResultModel response { get; set; }
        public GetNotificationResponseDefinition definition { get; set; }
    }


    public partial class GetNotificationResponseDefinition
    {
        public int totalCount { get; set; }
        public List<HandleNotificationDto> items { get; set; }
    }
}
