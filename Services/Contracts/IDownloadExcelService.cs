using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.Download;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IDownloadExcelService
    {
        Task<ResponseModelListManagementDto> GetAllManagementsFiltered(ManagementProfileInputFilterDto catalogInputCollectionDto);
        Task<DownloadWorktaskResponseDefinition> GetAllWorkTaskFiltered(WorkTaskDownloadDto catalogInputCollectionDto);
    }
    class DownloadExcelService : IDownloadExcelService
    {

        public HttpClient HttpClient { get; }
        public DownloadExcelService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        public async Task<ResponseModelListManagementDto> GetAllManagementsFiltered(ManagementProfileInputFilterDto catalogInputCollectionDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);


                var response = await HttpClient.PostAsJsonAsync($"/api/DownloadExcel/GetAllManagementsFiltered", catalogInputCollectionDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ResponseModelListManagementDto>();


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

        public async Task<DownloadWorktaskResponseDefinition> GetAllWorkTaskFiltered(WorkTaskDownloadDto catalogInputCollectionDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/DownloadExcel/GetAllWorkTaskFiltered", catalogInputCollectionDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<DownloadWorktaskResponseDefinition>();


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
