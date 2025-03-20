using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{

    public interface ICitizenManagment
    {
        Task<GetManagmentCitizenFilterResponse> CreateCitizenManagment(CreateManagementInputDto input, bool imageSetStreamContent = false);
    }


    public class CitizenManagment : ICitizenManagment
    {
        private const long MaxFileSize = 10240000L;

        public HttpClient HttpClient { get; }
        Blazored.LocalStorage.ILocalStorageService LocalStorage;

        public CitizenManagment(HttpClient httpClient)
        {
            HttpClient = httpClient;
          
        }

        public async Task<GetManagmentCitizenFilterResponse> CreateCitizenManagment(CreateManagementInputDto input, bool imageSetStreamContent)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                using var formData = new MultipartFormDataContent();
                // Add DTO properties to form data
                formData.Add(new StringContent(input.Description ?? ""), nameof(input.Description));
                formData.Add(new StringContent(input.Title ?? ""), nameof(input.Title));
                formData.Add(new StringContent(input.Status ?? ""), nameof(input.Status));
                if (input.Latitude != null && input.Longitude!=null)
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
                formData.Add(new StringContent(input.NameUserIncident ?? ""), nameof(input.NameUserIncident));

                if (imageSetStreamContent)
                {
                    if (input.streamContent != null && input.streamContent.Count > 0)
                    {
                        foreach (var file in input.streamContent)
                        {
                            
                            formData.Add(file.streamContentImage, "attachedFiles", file.FileImage.Name);
                        }
                    }
                }
                else
                {
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
                }
                var response = await HttpClient.PostAsync($"/api/Management/CreateExternalManagement", formData);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetManagmentCitizenFilterResponse>();
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
