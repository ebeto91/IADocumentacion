using DocumentFormat.OpenXml.Office2010.Excel;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskComments;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IWorkTaskCommentService
    {
        Task<ListWorkTaskCommentResponse> GetWorkTaskCommentsById(string id);

        Task<ListWorkTaskCommentResponse> CreateCommentForWorkTask(HandleWorkTaskCommentDto handleWorkTaskCommentDto);
        Task<ListWorkTaskCommentResponse> EditCommentForWorkTask(HandleWorkTaskCommentDto handleWorkTaskCommentDto);

    }
    public class WorkTaskCommentService : IWorkTaskCommentService
    {
        public HttpClient HttpClient { get; }

        public WorkTaskCommentService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<ListWorkTaskCommentResponse> GetWorkTaskCommentsById(string id)
        {

            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/WorkTaskComment/GetWorkTaskCommentsById/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ListWorkTaskCommentResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.Response != null)
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

        public async Task<ListWorkTaskCommentResponse> CreateCommentForWorkTask(HandleWorkTaskCommentDto handleWorkTaskCommentDto)
        {
            try
            {
                using var handleWorkTaskComment = new MultipartFormDataContent();
                // Use reflection to iterate over the properties of formData
                foreach (var property in handleWorkTaskCommentDto.GetType().GetProperties())
                {
                    var value = property.GetValue(handleWorkTaskCommentDto);
                    if (value != null)
                    {
                        if (property.Name != "AttachedNewFiles")
                        {
                            handleWorkTaskComment.Add(new StringContent(value?.ToString()), property.Name);
                        }

                    }
                }

                if (handleWorkTaskCommentDto.AttachedNewFiles != null)
                {
                    // Add files to form data
                    foreach (var file in handleWorkTaskCommentDto.AttachedNewFiles)
                    {
                        var fileStreamContent = new StreamContent(file.OpenReadStream());
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        handleWorkTaskComment.Add(fileStreamContent, "AttachedNewFiles", file.Name);
                    }
                }



                var response = await HttpClient.PostAsync($"/api/WorkTaskComment/CreateCommentForWorkTask", handleWorkTaskComment);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ListWorkTaskCommentResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.Response != null)
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

        public async Task<ListWorkTaskCommentResponse> EditCommentForWorkTask(HandleWorkTaskCommentDto handleWorkTaskCommentDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/WorkTaskComment/EditCommentForWorkTask", handleWorkTaskCommentDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ListWorkTaskCommentResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (responseData != null && responseData.Response != null)
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
