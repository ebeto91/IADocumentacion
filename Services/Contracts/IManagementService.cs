using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IManagementService
    {
        Task<ResponseModelProfileManagementDto> GetAllManagementsFiltered(ManagementProfileInputFilterDto catalogInputCollectionDto);
        Task<GetManagementInfoResponse> GetManagementById(string id);
        Task<GetManagementInfoResponse> GetManagementByPrincipalNumber(string id);
        Task<RatingManagementResponse> UpdateRating(RatingManagementDto ratingManagementDto);

        #region crud
        Task<GetManagementInfoResponse> AproveManagement(HandleManagementTaskDto handleManagementTaskDto);
        Task<GetManagementInfoResponse> DeniedManagement(HandleManagementTaskDto handleManagementTaskDto);
        Task<GetManagementInfoResponse> AssignCreateTaskWithManagement(AssingManagementTaskDto assingManagementTaskDto);
        Task<GetManagementInfoResponse> AssignCreateTaskWithoutManagement(AssingWithoutManagementTaskDto assingManagementTaskDto);
        #endregion
    }
    class ManagementService : IManagementService
    {


        public HttpClient _httpClient { get; }
        private const long MaxFileSize = 10240000L; // 10mb
        public ManagementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModelProfileManagementDto> GetAllManagementsFiltered(ManagementProfileInputFilterDto managementProfileInputFilterDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/Management/GetAllManagementsFiltered", managementProfileInputFilterDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<ResponseModelProfileManagementDto>();
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

        public async Task<GetManagementInfoResponse> GetManagementById(string id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.GetAsync($"/api/Management/GetManagementById/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagementInfoResponse>();
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


        public async Task<GetManagementInfoResponse> GetManagementByPrincipalNumber(string id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.GetAsync($"/api/Management/GetManagementByPrincipalNumber/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagementInfoResponse>();
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


        public async Task<RatingManagementResponse> UpdateRating(RatingManagementDto ratingManagementDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/Management/UpdateRating", ratingManagementDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<RatingManagementResponse>();

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



        public async Task<GetManagementInfoResponse> AproveManagement(HandleManagementTaskDto handleManagementTaskDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/Worktask/AproveManagement", handleManagementTaskDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagementInfoResponse>();
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

        public async Task<GetManagementInfoResponse> DeniedManagement(HandleManagementTaskDto handleManagementTaskDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/Worktask/DeniedManagement", handleManagementTaskDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagementInfoResponse>();
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

        public async Task<GetManagementInfoResponse> AssignCreateTaskWithManagement(AssingManagementTaskDto assingManagementTaskDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/Worktask/AssignCreateTaskWithManagement", assingManagementTaskDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagementInfoResponse>();
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

        public async Task<GetManagementInfoResponse> AssignCreateTaskWithoutManagement(AssingWithoutManagementTaskDto assingManagementTaskDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                using var handleWorkTaskComment = new MultipartFormDataContent();
                // Use reflection to iterate over the properties of formData
                foreach (var property in assingManagementTaskDto.GetType().GetProperties())
                {
                    var value = property.GetValue(assingManagementTaskDto);
                    if (value != null)
                    {


                        if (property.Name == "DueRateApplication" || property.Name == "StartDateApplication")
                        {
                            var valueDate = (DateTime)value;
                            var format = valueDate.ToString("yyyy-MM-dd");
                            handleWorkTaskComment.Add(new StringContent(format), property.Name);
                        }
                        else
                        {
                            if (property.Name != "attachedFiles" && property.Name != "UserDepartmentList")
                            {
                                handleWorkTaskComment.Add(new StringContent(value?.ToString()), property.Name);
                            }
                        }
                    }
                }


                if (assingManagementTaskDto.attachedFiles != null)
                {
                    // Add files to form data
                    foreach (var file in assingManagementTaskDto.attachedFiles)
                    {
                        var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        handleWorkTaskComment.Add(fileStreamContent, "attachedFiles", file.Name);
                    }
                }

                if (assingManagementTaskDto.UserDepartmentList != null)
                {

                    var listName = nameof(assingManagementTaskDto.UserDepartmentList);
                    for (int i = 0; i < assingManagementTaskDto.UserDepartmentList.Count(); i++)
                    {
                        var item = assingManagementTaskDto.UserDepartmentList[i];
                        foreach (var property in item.GetType().GetProperties())
                        {
                            var value = property.GetValue(item);
                            if (value != null)
                            {
                                var name = $"{listName}[{i}].{property.Name}";
                                handleWorkTaskComment.Add(new StringContent(value?.ToString()), name);

                            }
                        }
                    }
                }

                var response = await _httpClient.PostAsync($"/api/Worktask/AssignCreateTaskWithoutManagement", handleWorkTaskComment);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagementInfoResponse>();
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
