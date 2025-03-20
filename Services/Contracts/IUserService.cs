using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IUserService
    {
        #region CRUD
        Task<GetUserFilterResponse> GetUserFilters(UserInputDto userInputDto);
        Task<GetUserForListFilterResponse> GetUserForListFilter(UserInputDto userInputDto);
        Task<GetUserFilterResponse> CreateUser(ManagementUserDto managementUserDto);
        Task<GetUserFilterResponse> EditUser(ManagementUserDto managementUserDto);
        Task<GetUserFilterResponse> ActiveUser(UserInputControlDto userInputControlDto);
        Task<GetUserFilterResponse> InactiveUser(UserInputControlDto userInputControlDto);
        Task<GetUserWithRoleResponse> GetUserWithRole(UserInputControlDto userInputControlDto);


        #endregion


        Task<GetUserFilterResponse> CreateUserRegister(UserRegisterDto userRegisterDto);

        Task SubscribeToNotification(NotificationSubscription subscription);
        Task <GetUserFilterLogin> LoginUser(UserCredenditialsDto userCredenditialsDto);
        Task <GetUserFilterLogin> LoginUserAccessDirective(UserInfoAccessDirective userCredenditialsDto);

        Task<GetUserFilterResponse> UserChangePassword(RegeneratePasswordDto regeneratePasswordDto);
        Task<GetUserFilterResponse> ChangePassword(ChangePasswordDto changePasswordDto);

        Task<GetUserFilterResponse> CompleteRegister(RegisterCompleteDto registerCompleteDto);


        Task<GetAllUserResponseDto> GetAllUserResponse(UserIdentificationInputDto input);

        #region User Profile

        Task<UserProfileResponse> GetUserProfile(Guid id);
        Task<ResponseModelUserProfile> UpdateUserProfile(UserProfileResponse input);

        Task<ResponseModel<UserAttachedPhotoInputDto>> UpdateImageUserProfile(UpdateImageUserProfile updateImageUserProfile);

        #endregion


    }

    class UserService : IUserService
    {
        public HttpClient HttpClient { get; }
        Blazored.LocalStorage.ILocalStorageService LocalStorage;
        private const long MaxFileSize = 10240000L;

        public CustomAuthService _customAuthService { get; set; }
        public UserService(HttpClient httpClient, Blazored.LocalStorage.ILocalStorageService localStorage, CustomAuthService customAuthService)
        {
            HttpClient = httpClient;
            LocalStorage = localStorage;
            _customAuthService = customAuthService;
        }
        #region listado e item

        public async Task<GetUserFilterResponse> GetUserFilters(UserInputDto userInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/User/GetUsersFilter", userInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var data2 = await response.Content.ReadAsStringAsync();
                var listOfInstances = data2.FromJson<GetUserFilterResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (listOfInstances != null && listOfInstances.definition != null)
                {
                    return listOfInstances;
                }
                else
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetUserForListFilterResponse> GetUserForListFilter(UserInputDto userInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/User/GetUsersFilter", userInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var listOfInstances = responseContent.FromJson<GetUserForListFilterResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (listOfInstances != null && listOfInstances.definition != null)
                {
                    return listOfInstances;
                }
                else
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        public async Task<GetUserWithRoleResponse> GetUserWithRole(UserInputControlDto userInputControlDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/User/GetUser/{userInputControlDto.Id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var dataResponse = responseContent.FromJson<GetUserWithRoleResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (dataResponse != null && dataResponse.definition != null)
                {
                    return dataResponse;
                }
                else
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }
        #endregion
        #region active / inactive

        public async Task<GetUserFilterResponse> ActiveUser(UserInputControlDto userInputControlDto)
        {

            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PutAsJsonAsync($"/api/User/ActiveUser/{userInputControlDto.Id}", userInputControlDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var instance = responseContent.FromJson<GetUserFilterResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (instance != null && instance.response != null)
                {
                    return instance;
                }
                else
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }

        }

        public async Task<GetUserFilterResponse> InactiveUser(UserInputControlDto userInputControlDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PutAsJsonAsync($"/api/User/InactiveUser/{userInputControlDto.Id}", userInputControlDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var instance = responseContent.FromJson<GetUserFilterResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (instance != null && instance.response != null)
                {
                    return instance;
                }
                else
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
                }

            }
            catch (Exception ex)
            {
                return null;
                //return GetDefaultErrorMessage<GetUserFilterResponse>("Ha Ocurrido Un Error");
            }
        }

        #endregion
        public async Task<GetUserFilterResponse> CreateUser(ManagementUserDto managementUserDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/User/PostUser", managementUserDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetUserFilterResponse>();
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

        public async Task<GetUserFilterResponse> EditUser(ManagementUserDto managementUserDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/User/PutUser", managementUserDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetUserFilterResponse>();
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


        public async Task SubscribeToNotification(NotificationSubscription subscription)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/Notifications/Subscribe", subscription);
            try
            {
                var data2 = await response.Content.ReadAsStringAsync();
            }
            catch (System.Exception ex)
            {

                var sa = "";
            }


        }

        public async Task<GetUserFilterLogin> LoginUser(UserCredenditialsDto userCredenditialsDto)
        {
            var response = await HttpClient.PostAsJsonAsync($"/api/Access/Login", userCredenditialsDto);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse= await response.Content.ReadAsStringAsync();
                var responseData = stringResponse.FromJson<GetUserFilterLogin>();
                GetUserFilterResponse getUserFilterResponse = new GetUserFilterResponse();
                if (!string.IsNullOrEmpty(stringResponse))
                {
                    if (responseData != null && responseData.definition != null)
                    {
                        var token = responseData.definition.Token;
                        //await LocalStorage.SetItemAsStringAsync("authToken", token);
                        //((CustomAuthSatateProvider)_customAuthSatateProvider).UserIsAuthenticated();
                        await _customAuthService.LoginAsync(token);
                    }
                   
                    return responseData;
                }
                else
                {
                   //getUserFilterResponse.response.Success = false;
                    //getUserFilterResponse.response.Message = "Problemas al loguear";
                    return responseData;
                  
                }
            }
            else
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseData = stringResponse.FromJson<GetUserFilterLogin>();
               // GetUserFilterResponse getUserFilterResponse = new GetUserFilterResponse();
              //  getUserFilterResponse.response.Success = false;
                //getUserFilterResponse.response.Message = "Problemas al loguear";

                return responseData;
            }
        }

        public async Task<GetUserFilterLogin> LoginUserAccessDirective(UserInfoAccessDirective userCredenditialsDto)
        {
            var response = await HttpClient.PostAsJsonAsync($"/api/AccessDirective/LoginUser", userCredenditialsDto);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseData = stringResponse.FromJson<GetUserFilterLogin>();
                GetUserFilterResponse getUserFilterResponse = new GetUserFilterResponse();
                if (!string.IsNullOrEmpty(stringResponse))
                {
                    if (responseData != null && responseData.definition != null)
                    {
                        var token = responseData.definition.Token;
                        //await LocalStorage.SetItemAsStringAsync("authToken", token);
                        //((CustomAuthSatateProvider)_customAuthSatateProvider).UserIsAuthenticated();
                        await _customAuthService.LoginAsync(token);
                    }

                    return responseData;
                }
                else
                {
                    //getUserFilterResponse.response.Success = false;
                    //getUserFilterResponse.response.Message = "Problemas al loguear";
                    return responseData;

                }
            }
            else
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseData = stringResponse.FromJson<GetUserFilterLogin>();
                // GetUserFilterResponse getUserFilterResponse = new GetUserFilterResponse();
                //  getUserFilterResponse.response.Success = false;
                //getUserFilterResponse.response.Message = "Problemas al loguear";

                return responseData;
            }
        }

        public async Task<GetUserFilterResponse> CreateUserRegister(UserRegisterDto userRegisterDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                userRegisterDto.Enabled = true;
                userRegisterDto.UserName = "";
                userRegisterDto.UserType = USERTYPE.EXTERNAL;
                userRegisterDto.PhoneNumber = string.IsNullOrEmpty(userRegisterDto.PhoneNumber) ? "" : userRegisterDto.PhoneNumber;

                var response = await HttpClient.PostAsJsonAsync($"/api/Access/RegisterUser", userRegisterDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetUserFilterResponse>();
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

        public async Task<GetUserFilterResponse> UserChangePassword(RegeneratePasswordDto regeneratePasswordDto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/Access/SendChangePassword", regeneratePasswordDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetUserFilterResponse>();

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
            }


        }

        public async Task<GetUserFilterResponse> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/Access/ChangePasswordComplete", changePasswordDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetUserFilterResponse>();
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

        public async Task<GetUserFilterResponse> CompleteRegister(RegisterCompleteDto registerCompleteDto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/Access/RegisterUserComplete", registerCompleteDto);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetUserFilterResponse>();

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
            }

        }




        public async Task<UserProfileResponse> GetUserProfile(Guid id)
        {

            try
            {
                var response = await HttpClient.GetAsync($"/api/User/GetUserProfile/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var dataResponse = responseContent.FromJson<ResponseModelUserProfile>();


                if (dataResponse != null && dataResponse.Definition != null)
                {
                    return dataResponse.Definition;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseModelUserProfile> UpdateUserProfile(UserProfileResponse input)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/User/UpdateUserProfile", input);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var dataResponse = responseContent.FromJson<ResponseModelUserProfile>();


                if (dataResponse != null && dataResponse.Definition != null)
                {
                    return dataResponse;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseModel<UserAttachedPhotoInputDto>> UpdateImageUserProfile(UpdateImageUserProfile updateImageUserProfile)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                content.Add(new StringContent(updateImageUserProfile.UserId.ToString()), "UserId");

                var fileContent = new StreamContent(updateImageUserProfile.AttachedFile.OpenReadStream(MaxFileSize));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(updateImageUserProfile.AttachedFile.ContentType);
                content.Add(fileContent, "AttachedFile", updateImageUserProfile.AttachedFile.Name);

                var response = await HttpClient.PostAsync($"/api/User/UpdateImageUserProfile", content);
                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var dataResponse = responseContent.FromJson<ResponseModel<UserAttachedPhotoInputDto>>();
                if (dataResponse != null && dataResponse.Definition != null)
                {
                    return dataResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<GetAllUserResponseDto> GetAllUserResponse(UserIdentificationInputDto input)
        {
            try
            {
                
                var response = await HttpClient.PostAsJsonAsync($"/api/User/GetAllUsers",input);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var dataResponse = responseContent.FromJson<GetAllUserResponseDto>();


                if (dataResponse != null && dataResponse.definition != null)
                {
                    return dataResponse;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
