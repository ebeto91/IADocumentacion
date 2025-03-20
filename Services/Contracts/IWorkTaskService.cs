using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskComments;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskKanban;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IWorkTaskService
    {
        Task<ResponseConvertSurveyToTask> AssignCreateTaskForVoteSurvey(HangleManagementTaskBySurvey managementDto);
        Task<GetWorkTaskForUsersAssignedResponse> GetWorkTaskForUsersAssignedFilter(WorkTaskInputDto workTaskInputDto);
        Task<WorkTaskKanbanResponseDefinition> GetWorkTaskFilterForKanban(WorkTaskKanbanInputDto workTaskInputDto);
        Task<GetWorkTaskByIdResponseDefinition> GetWorkTaskById(string id);
        Task<GetWorkTaskByIdResponseDefinition> GetUserAllowToEdit(string id);
        Task<GetWorkTaskByIdResponseDefinition> EditWorkTask(WorkTaskInputEditTrackableDto workTaskInputEditDto);

        Task<GetWorkTaskByIdResponseDefinition> RaiseMemoryToWorktask(HandleRaiseMemoryToWorktask input);
        Task<HandleChangeUsersAssingWorkTaskResponse> ChangeUserAssignedToWorkTask(HandleChangeUsersAssingWorkTask workTaskInputEditDto);
    }
    public class WorkTaskService : IWorkTaskService
    {
        public HttpClient _httpClient { get; }

        public WorkTaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private const long MaxFileSize = 10240000L;

        public async Task<GetWorkTaskForUsersAssignedResponse> GetWorkTaskForUsersAssignedFilter(WorkTaskInputDto workTaskInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/WorkTask/GetWorkTaskForUsersAssignedFilter", workTaskInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetWorkTaskForUsersAssignedResponse>();
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


        public async Task<WorkTaskKanbanResponseDefinition> GetWorkTaskFilterForKanban(WorkTaskKanbanInputDto workTaskInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/WorkTask/GetWorkTaskFilterForKanban", workTaskInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<WorkTaskKanbanResponseDefinition>();
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

        public async Task<GetWorkTaskByIdResponseDefinition> GetWorkTaskById(string id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.GetAsync($"/api/WorkTask/GetWorkTaskById/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetWorkTaskByIdResponseDefinition>();
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


        public async Task<GetWorkTaskByIdResponseDefinition> GetUserAllowToEdit(string id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.GetAsync($"/api/WorkTask/GetUserAllowToEdit/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetWorkTaskByIdResponseDefinition>();
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



        public async Task<ResponseConvertSurveyToTask> AssignCreateTaskForVoteSurvey(HangleManagementTaskBySurvey managementDto)
        {
            try { 
            using var handleWorkTaskComment = new MultipartFormDataContent();

            foreach (var property in managementDto.GetType().GetProperties())
            {
                var value = property.GetValue(managementDto);
                if (value != null)
                {


                    if (property.Name == "DueRateApplication" || property.Name == "StartDateApplication" || property.Name == "FollowDateReminderToCreatorUser")
                    {
                        var valueDate = (DateTime)value;
                        var format = valueDate.ToString("yyyy-MM-dd");
                        handleWorkTaskComment.Add(new StringContent(format), property.Name);
                    }
                    else
                    {
                        if (property.Name != "AttachedNewFiles" && property.Name != "AttachedDocuments")
                        {
                            handleWorkTaskComment.Add(new StringContent(value?.ToString()), property.Name);
                        }
                    }
                }
            }
            var response = await _httpClient.PostAsync($"/api/WorkTask/AssignCreateTaskForVoteSurvey", handleWorkTaskComment);

            if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
            }
            var contentString = await response.Content.ReadAsStringAsync();
            var responseData = contentString.FromJson<ResponseConvertSurveyToTask>();
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



        public async Task<GetWorkTaskByIdResponseDefinition> EditWorkTask(WorkTaskInputEditTrackableDto workTaskInputEditDto)
        {
            try
            {
                using var handleWorkTaskComment = new MultipartFormDataContent();
                // Use reflection to iterate over the properties of formData
                foreach (var property in workTaskInputEditDto.GetType().GetProperties())
                {
                    var value = property.GetValue(workTaskInputEditDto);
                    if (value != null)
                    {
                       

                        if (property.Name == "DueRateApplication" || property.Name == "StartDateApplication" || property.Name == "FollowDateReminderToCreatorUser")
                        {
                            var valueDate = (DateTime)value;
                            var format = valueDate.ToString("yyyy-MM-dd");
                            handleWorkTaskComment.Add(new StringContent(format), property.Name);
                        }
                        else
                        {
                            if (property.Name != "AttachedNewFiles" && property.Name != "AttachedDocuments")
                            {
                                handleWorkTaskComment.Add(new StringContent(value?.ToString()), property.Name);
                            }
                        }
                    }
                }

                if (workTaskInputEditDto.AttachedDocuments != null)
                {

                    var listName = nameof(workTaskInputEditDto.AttachedDocuments);
                    for (int i = 0; i < workTaskInputEditDto.AttachedDocuments.Count(); i++)
                    {
                        var item = workTaskInputEditDto.AttachedDocuments[i];
                        foreach (var property in item.GetType().GetProperties())
                        {
                            var value = property.GetValue(item);
                            if (value != null)
                            {
                                var name = $"{listName}[{i}].{ property.Name}";
                                handleWorkTaskComment.Add(new StringContent(value?.ToString()), name);

                            }
                        }
                    }
                }


                if (workTaskInputEditDto.AttachedNewFiles != null)
                {
                    // Add files to form data
                    foreach (var file in workTaskInputEditDto.AttachedNewFiles)
                    {
                        var fileStreamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        handleWorkTaskComment.Add(fileStreamContent, "AttachedNewFiles", file.Name);
                    }
                }



                var response = await _httpClient.PostAsync($"/api/WorkTask/EditWorkTask", handleWorkTaskComment);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetWorkTaskByIdResponseDefinition>();
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

        public async Task<HandleChangeUsersAssingWorkTaskResponse> ChangeUserAssignedToWorkTask(HandleChangeUsersAssingWorkTask handleChangeUsersAssing)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/WorkTask/ChangeUserAssignedToWorkTask", handleChangeUsersAssing);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<HandleChangeUsersAssingWorkTaskResponse>();
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

        public async Task<GetWorkTaskByIdResponseDefinition> RaiseMemoryToWorktask(HandleRaiseMemoryToWorktask input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await _httpClient.PostAsJsonAsync($"/api/WorkTask/RaiseMemoryToWorktask", input);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetWorkTaskByIdResponseDefinition>();
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
