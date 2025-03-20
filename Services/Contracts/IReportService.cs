using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Report;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IReportService
    {
        Task<GetReportInformationDto> GetInformationReport();
    }

    public class ReportService : IReportService
    {
        public HttpClient HttpClient { get; }
        public ReportService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<GetReportInformationDto> GetInformationReport()
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/Report/GetInformationReport");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetReportInformationDto>();


                if (responseData != null && responseData.definition != null)
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
