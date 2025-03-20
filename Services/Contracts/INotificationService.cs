using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Notifications;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> GetActiveMessages(NotificationInputDto notificationInputDto);
        Task<NotificationResponseDto> GetAllActiveMessages(NotificationInputBellDto notificationInputDto);
        Task<NotificationResponseDto> MarkAsReadNotification(HandleNotificationMarkAsRead handleNotificationMarkAsRead);
    }

    public class NotificationService : INotificationService
    {

        public HttpClient HttpClient { get; }
        public NotificationService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<NotificationResponseDto> GetActiveMessages(NotificationInputDto notificationInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/Notification/GetActiveMessages", notificationInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<NotificationResponseDto>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }


        public async Task<NotificationResponseDto> GetAllActiveMessages(NotificationInputBellDto notificationInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/Notification/GetAllActiveMessages", notificationInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<NotificationResponseDto>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<NotificationResponseDto> MarkAsReadNotification(HandleNotificationMarkAsRead handleNotificationMarkAsRead)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/Notification/MarkAsReadNotification", handleNotificationMarkAsRead);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<NotificationResponseDto>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.response != null)
                {
                    return responseData;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }
    }
}
