using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRoles();
        #region crud
        Task<GetRolListResponse> GetRolesFilterData(RoleFilterRequest roleFilterRequest);
        Task<RolListResponseDefinition> GetRolesFilterDataForConfig(RoleFilterRequestInput roleFilter);
        Task<GetRolProcessResponse> ActiveRole(RoleFilter roleFilter);
        Task<GetRolProcessResponse> InactiveRole(RoleFilter roleFilter);
        Task<List<MenuModuleRolConfigDto>> GetScopesDataGroup();
        Task<RoleForEditDto> GetRoleForEditById(RoleFilter rol);

        Task<GetRolProcessResponse> CreateRole(RoleCategoryScopeSave roleCategoryScopeSave);
        Task<GetRolProcessResponse> EditRole(RoleCategoryScopeSave roleCategoryScopeSave);
        #endregion
    }
    public class RoleService : IRoleService
    {
        public HttpClient HttpClient { get; }
        public RoleService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        #region listado
        
        public async Task<List<RoleDto>> GetRoles()
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/Role/GetRoles");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetRolesResponse>();


                if (responseData != null && responseData.definition != null && responseData.definition.Count > 0)
                {
                    return responseData.definition;
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

        public async Task<GetRolListResponse> GetRolesFilterData(RoleFilterRequest roleFilterRequest)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Role/GetRolesFilterData", roleFilterRequest);

                if (response != null && !response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetRolListResponse>();
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
        public async Task<RolListResponseDefinition> GetRolesFilterDataForConfig(RoleFilterRequestInput roleFilter)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Role/GetRolesFilterDataForConfig", roleFilter);

                if (response != null && !response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetRolListResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (listOfInstances != null && listOfInstances.definition != null)
                {
                    return listOfInstances.definition;
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
        #region activar / inactivar
        public async Task<GetRolProcessResponse> ActiveRole(RoleFilter roleFilter)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PutAsJsonAsync($"/api/Role/ActiveRole/{roleFilter.IdRole}", roleFilter);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var contentData = dataString.FromJson<GetRolProcessResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (contentData != null && contentData.response != null)
                {
                    return contentData;
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

        public async Task<GetRolProcessResponse> InactiveRole(RoleFilter roleFilter)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PutAsJsonAsync($"/api/Role/InactiveRole/{roleFilter.IdRole}", roleFilter);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var contentData = dataString.FromJson<GetRolProcessResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (contentData != null && contentData.response != null)
                {
                    return contentData;
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
        #region get data for edit / create 
     
        public async Task<List<MenuModuleRolConfigDto>> GetScopesDataGroup()
        {

            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                RoleFilter roleFilter = new RoleFilter();
                var response = await HttpClient.PostAsJsonAsync($"/api/Role/GetScopesDataGroup", roleFilter);

                if (response != null && !response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var contentData = dataString.FromJson<MenuModuleRolConfigScopeResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (contentData != null && contentData.definition != null)
                {
                    return contentData.definition.MenuModuleRolConfigDto;
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

        public async Task<GetRolProcessResponse> CreateRole(RoleCategoryScopeSave roleCategoryScopeSave)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                RoleFilter roleFilter = new RoleFilter();
                var response = await HttpClient.PostAsJsonAsync($"/api/Role/CreateRole", roleCategoryScopeSave);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;

                }
                var dataString = await response.Content.ReadAsStringAsync();
                var contentData = dataString.FromJson<GetRolProcessResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (contentData != null && contentData.response != null)
                {
                    return contentData;
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

        public async Task<RoleForEditDto> GetRoleForEditById(RoleFilter rol)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/Role/GetRoleForEditById", rol);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var contentData = dataString.FromJson<GetRolByIdResponse>();


                if (contentData != null && contentData.definition != null)
                {
                    return contentData.definition;
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

        public async Task<GetRolProcessResponse> EditRole(RoleCategoryScopeSave roleCategoryScopeSave)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                RoleFilter roleFilter = new RoleFilter();
                var response = await HttpClient.PostAsJsonAsync($"/api/Role/EditRole", roleCategoryScopeSave);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;

                }
                var dataString = await response.Content.ReadAsStringAsync();
                var contentData = dataString.FromJson<GetRolProcessResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (contentData != null && contentData.response != null)
                {
                    return contentData;
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
    }
}
