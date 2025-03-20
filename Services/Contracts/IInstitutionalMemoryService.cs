using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Headers;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IInstitutionalMemoryService
    {
        Task<InstitutionalMemoryResponse> CreateInstitutionalMemory(CreateManagementInputDto input);
    }

    public class InstitutionalMemoryService : IInstitutionalMemoryService
    {
        public HttpClient HttpClient { get;  }
        private const long MaxFileSize = 10240000L;

        public InstitutionalMemoryService(HttpClient httpClient)
        {
            HttpClient = httpClient;

        }

       public async Task<InstitutionalMemoryResponse> CreateInstitutionalMemory(CreateManagementInputDto input)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                using var formData = new MultipartFormDataContent();
                // Add DTO properties to form data
                formData.Add(new StringContent(input.Description ?? ""), nameof(input.Description));
                formData.Add(new StringContent(input.Title ?? ""), nameof(input.Title));
                formData.Add(new StringContent(input.Status ?? ""), nameof(input.Status));
                if (input.Latitude != null && input.Longitude != null)
                {
                    formData.Add(new StringContent(input.Latitude?.ToString() ?? ""), nameof(input.Latitude));
                    formData.Add(new StringContent(input.Longitude?.ToString() ?? ""), nameof(input.Longitude));
                }
                formData.Add(new StringContent(input.District ?? ""), nameof(input.District));
                formData.Add(new StringContent(input.Neighborhood ?? ""), nameof(input.Neighborhood));
                if (input.AssociationRelatedMemoryId != null)
                {
                    formData.Add(new StringContent(input.AssociationRelatedMemoryId?.ToString() ?? ""), nameof(input.AssociationRelatedMemoryId));
                }
                formData.Add(new StringContent(input.ContactPoint ?? ""), nameof(input.ContactPoint));
                formData.Add(new StringContent(input.TypeCreation ?? ""), nameof(input.TypeCreation));
                formData.Add(new StringContent(input.PrincipalTypeApplication ?? ""), nameof(input.PrincipalTypeApplication));
                formData.Add(new StringContent(input.TypeApplication ?? ""), nameof(input.TypeApplication));
                formData.Add(new StringContent(input.IsAnonymous.ToString()), nameof(input.IsAnonymous));
                if (input.DateIndicident != null)
                {
                    formData.Add(new StringContent(((DateTime)input.DateIndicident).ToString("yyyy-MM-dd")), nameof(input.DateIndicident));
                }
                formData.Add(new StringContent(input.AllowContact.ToString()), nameof(input.AllowContact));
                formData.Add(new StringContent(input.CreatedUserIpAddress ?? ""), nameof(input.CreatedUserIpAddress));
                formData.Add(new StringContent(input.ManagementName ?? ""), nameof(input.ManagementName));
                formData.Add(new StringContent(input.ManagementEmail ?? ""), nameof(input.ManagementEmail));
                formData.Add(new StringContent(input.ManagementPhone ?? ""), nameof(input.ManagementPhone));
                formData.Add(new StringContent(input.ExternalManagementName ?? ""), nameof(input.ExternalManagementName));
                formData.Add(new StringContent(input.ExternalManagementEmail ?? ""), nameof(input.ExternalManagementEmail));
                formData.Add(new StringContent(input.ExternalManagementPhone ?? ""), nameof(input.ExternalManagementPhone));

                // Add files to form data
                if (input.attachedFiles != null && input.attachedFiles.Count > 0)
                {
                    foreach (var file in input.attachedFiles)
                    {
                        var fileStreamContent = new StreamContent(file.OpenReadStream(maxAllowedSize: MaxFileSize));
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        formData.Add(fileStreamContent, "attachedFiles", file.Name);
                    }
                }

                if (input.FollowDateReminderToCreatorUser != null)
                {
                    formData.Add(new StringContent(((DateTime)input.FollowDateReminderToCreatorUser).ToString("yyyy-MM-dd")), nameof(input.FollowDateReminderToCreatorUser));
                }

             
                    formData.Add(new StringContent(input.ApplicableBudget ?? ""), nameof(input.ApplicableBudget));
                

              
                formData.Add(new StringContent(input.Priority ?? ""), nameof(input.Priority));
                

                if (input.StartDateApplication != null)
                {
                    formData.Add(new StringContent(((DateTime)input.StartDateApplication).ToString("yyyy-MM-dd")), nameof(input.StartDateApplication));
                }

                if (input.DueRateApplication != null)
                {
                    formData.Add(new StringContent(((DateTime)input.DueRateApplication).ToString("yyyy-MM-dd")), nameof(input.DueRateApplication));
                }

               


                if (input.UserDepartmentList != null && input.UserDepartmentList.Count>0)
                {

                    var listName = nameof(input.UserDepartmentList);
                    for (int i = 0; i < input.UserDepartmentList.Count(); i++)
                    {
                        var item = input.UserDepartmentList[i];
                        foreach (var property in item.GetType().GetProperties())
                        {
                            var value = property.GetValue(item);
                            if (value != null)
                            {
                                var name = $"{listName}[{i}].{property.Name}";
                                formData.Add(new StringContent(value?.ToString()), name);

                            }
                        }
                    }
                }


                var response = await HttpClient.PostAsync($"/api/WorkTask/AssignCreateTaskByMemory", formData);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<InstitutionalMemoryResponse>();
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
