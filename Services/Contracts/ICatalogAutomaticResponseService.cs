using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{

    public interface ICatalogAutomaticResponseService
    {
        Task<GetCatalogAutomaticResponse> GetListFilters(CatalogResponseInputDto catalogResponseInputDto);
        Task<PostCatalogAutomaticResponse> PostCatalogResponse(ManagementCatalogResponse catalogResponseInputDto);
        Task<PostCatalogAutomaticResponse> PutCatalogResponse(ManagementCatalogResponse catalogResponseInputDto);
        Task<PostCatalogAutomaticResponse> DeleteCatalogResponse(Guid id);
        Task<PostCatalogAutomaticResponse> PostMassiveAutomaticResponse(List<CatalogAutomaticResponseDto> listHandleAutomaticResponses);
        Task<GetCatalogResponseForListResponse>  GetCatalogResponseForList(CatalogResponseInputListDto catalogResponseInputListDto);
        Task<GetCatalogResponseForListResponse> GetCatalogResponseForListByCodesList(CatalogResponseInputListMultipleDto catalogResponseInputListDto);
        //Task<GetUserFilterResponse> CreateUser(ManagementUserDto managementUserDto);
    }

    class CatalogAutomaticResponseService : ICatalogAutomaticResponseService
    {
        public HttpClient HttpClient { get; }
        Blazored.LocalStorage.ILocalStorageService LocalStorage;

        public CatalogAutomaticResponseService(HttpClient httpClient, Blazored.LocalStorage.ILocalStorageService localStorage)
        {
            HttpClient = httpClient;
            LocalStorage = localStorage;
        }

        public async Task<GetCatalogAutomaticResponse> GetListFilters(CatalogResponseInputDto catalogResponseInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Catalog/GetCatalogReponseFilter", catalogResponseInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetCatalogAutomaticResponse>();
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

        public async Task<PostCatalogAutomaticResponse> PostCatalogResponse(ManagementCatalogResponse catalogResponseInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/Catalog/PostCatalogResponse", catalogResponseInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostCatalogAutomaticResponse>();


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

        public async Task<PostCatalogAutomaticResponse> PutCatalogResponse(ManagementCatalogResponse catalogResponseInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/Catalog/PutCatalogResponse", catalogResponseInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostCatalogAutomaticResponse>();


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


        public async Task<PostCatalogAutomaticResponse> DeleteCatalogResponse(Guid id)
        {
            try
            {

                var response = await HttpClient.DeleteAsync($"/api/Catalog/DeleteCatalogResponse/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostCatalogAutomaticResponse>();


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

        public async Task<PostCatalogAutomaticResponse> PostMassiveAutomaticResponse(List<CatalogAutomaticResponseDto> listHandleAutomaticResponses)
        {

            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/Catalog/PostMassiveAutomaticResponse", listHandleAutomaticResponses);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostCatalogAutomaticResponse>();


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


        public async Task<GetCatalogResponseForListResponse> GetCatalogResponseForList(CatalogResponseInputListDto catalogResponseInputListDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Catalog/GetCatalogResponseForList", catalogResponseInputListDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetCatalogResponseForListResponse>();
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


        public async Task<GetCatalogResponseForListResponse> GetCatalogResponseForListByCodesList(CatalogResponseInputListMultipleDto catalogResponseInputListDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Catalog/GetCatalogResponseForListByCodesList", catalogResponseInputListDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetCatalogResponseForListResponse>();
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
    }
}
