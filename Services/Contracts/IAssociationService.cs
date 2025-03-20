using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{


    public interface IAssociationService
    {
        Task<GetAssociationResponse> GetListFilters(AssociationInputDto associationInputDto);
        Task<PostAssociationProcessResponse> PostAssociation(HandleAssociationConfig handleAssociationConfig);
        Task<PostAssociationProcessResponse> PostMassiveAssociation(List<HandleAssociationConfig> listHandleAssociationConfig);
        Task<PostAssociationProcessResponse> PutAssociation(HandleAssociationConfig handleAssociationConfig);
        Task<PostAssociationProcessResponse> DeleteAssociation(Guid id);
        Task<GetAssociationListUsersResponse> GetUsersForJoinAsociation(AssociationIdInputDto associationIdInputDto);
        Task<PostAssociationProcessResponse> PostAssociationJoinUser(HandleAssociationJoinUser handleAssociationJoinUser);
        Task<PostAssociationProcessResponse> PostAssociationUnjoinUser(HandleAssociationJoinUser handleAssociationJoinUser);

        Task<AssociationGroupByDistrictResponse> GetAssociationGroupByDistrict();
        //Task<GetUserFilterResponse> CreateUser(ManagementUserDto managementUserDto);
    }

    class AssociationService : IAssociationService
    {
        public HttpClient HttpClient { get; }

        public AssociationService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<GetAssociationResponse> GetListFilters(AssociationInputDto associationInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/AssociationDistrict/GetAssociationsFilter", associationInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetAssociationResponse>();
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



        public async Task<PostAssociationProcessResponse> PostAssociation(HandleAssociationConfig handleAssociationConfig)
        {
            try
            {
                handleAssociationConfig.Name = handleAssociationConfig.Name.Trim();
                var response = await HttpClient.PostAsJsonAsync($"/api/AssociationDistrict/PostAssociation", handleAssociationConfig);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostAssociationProcessResponse>();


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

        public async Task<PostAssociationProcessResponse> PostMassiveAssociation(List<HandleAssociationConfig> listHandleAssociationConfig)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/AssociationDistrict/PostMassiveAssociation", listHandleAssociationConfig);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostAssociationProcessResponse>();


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

        public async Task<PostAssociationProcessResponse> PutAssociation(HandleAssociationConfig handleAssociationConfig)
        {
            try
            {
                handleAssociationConfig.Name = handleAssociationConfig.Name.Trim();
                var response = await HttpClient.PutAsJsonAsync($"/api/AssociationDistrict/PutAssociation/{handleAssociationConfig.Id}", handleAssociationConfig);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostAssociationProcessResponse>();


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

        public async Task<PostAssociationProcessResponse> DeleteAssociation(Guid id)
        {
            try
            {

                var response = await HttpClient.DeleteAsync($"/api/AssociationDistrict/DeleteAssociation/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostAssociationProcessResponse>();


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

        public async Task<GetAssociationListUsersResponse> GetUsersForJoinAsociation(AssociationIdInputDto associationIdInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/AssociationDistrict/GetUsersForJoinAsociation", associationIdInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetAssociationListUsersResponse>();


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

        public async Task<PostAssociationProcessResponse> PostAssociationJoinUser(HandleAssociationJoinUser associationIdInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/AssociationDistrict/PostAssociationJoinUser", associationIdInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostAssociationProcessResponse>();


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

        public async Task<PostAssociationProcessResponse> PostAssociationUnjoinUser(HandleAssociationJoinUser associationIdInputDto)
        {
            try
            {

                var response = await HttpClient.PostAsJsonAsync($"/api/AssociationDistrict/PostAssociationUnjoinUser", associationIdInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<PostAssociationProcessResponse>();


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

        public async Task<AssociationGroupByDistrictResponse> GetAssociationGroupByDistrict()
        {
            try
            {

                var response = await HttpClient.GetAsync($"/api/AssociationDistrict/GetAllAssociationsByDistrictCode");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<AssociationGroupByDistrictResponse>();


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
