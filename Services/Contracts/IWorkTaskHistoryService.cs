using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskComments;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskHistory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IWorkTaskHistoryService
    {
        Task<WorkTaskResponseHistoryDtoGeneralResponse> GetWorkTaskHistoryDetails(string id);
    }

    public class WorkTaskHistoryService : IWorkTaskHistoryService
    {

        public HttpClient HttpClient { get; }


        public WorkTaskHistoryService(HttpClient httpClient)
        {
            HttpClient = httpClient;

        }

        public async Task<WorkTaskResponseHistoryDtoGeneralResponse> GetWorkTaskHistoryDetails(string id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                WorkTaskInputHistory workTaskInputHistory = new WorkTaskInputHistory()
                {
                    Id = id

                };
                var response = await HttpClient.PostAsJsonAsync($"/api/WorkTaskHistory/GetAllHistoriesByWorkTaskId", workTaskInputHistory);

                if (response != null && !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<WorkTaskResponseHistoryDtoGeneralResponse>();
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
