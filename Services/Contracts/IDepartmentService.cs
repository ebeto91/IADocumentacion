using DocumentFormat.OpenXml.Office2010.Excel;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IDepartmentService
    {
        Task<GetDepartmentListResponse> GetListFilters(DepartmentInputDto departmentInputDto);
        Task<GetDepartmentGeneralResponse> PostDepartment(ManagementDepartmentDto managementDepartmentDto);
        Task<GetDepartmentGeneralResponse> PostMassiveDepartament(List<DepartmenstExcelDto> listHandleAssociationConfig);
        Task<GetDepartmentGeneralResponse> PutDepartment(ManagementDepartmentDto managementDepartmentDto);
        Task<GetDepartmentGeneralResponse> DeleteDepartment(Guid id);
        Task<GetDepartmentGeneralResponse> GetDeparmentById(string id);
        #region users
        Task<GetUserBasicResponse> GetUsersForAssign();
        Task<GetUserBasicResponse> GetUsersForAssignForEdit(DepartmentIdInputDto departmentIdInputDto);
        Task<GetDepartmentUsersResponseGeneral> GetDeparmentWithUsers(DepartmentIdInputDto departmentIdInputDto);
        Task<GetResponseAssignUsersGeneral> PostAssingUsersDepartment(AssingUserDepartmentInputDto assingUserDepartmentInputDto);
        #endregion
        #region assign users to worktask

        Task<GetDeparmentsWithUsersForWorkTaskResponse> GetDeparmentsWithUsers();
        Task<GetDepartmentListResponse> GetAllDepartments();
        #endregion
    }
    public class DepartmentService : IDepartmentService
    {
        public HttpClient HttpClient { get; }

        public DepartmentService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        #region crud
        public async Task<GetDepartmentListResponse> GetListFilters(DepartmentInputDto departmentInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Department/GetDepartmentsFilter", departmentInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetDepartmentListResponse>();
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

        public async Task<GetDepartmentGeneralResponse> PostDepartment(ManagementDepartmentDto managementDepartmentDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PostAsJsonAsync($"/api/Department/PostDepartment", managementDepartmentDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var informationData = dataString.FromJson<GetDepartmentGeneralResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (informationData != null && informationData.response != null)
                {
                    return informationData;
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

        public async Task<GetDepartmentGeneralResponse> PutDepartment(ManagementDepartmentDto managementDepartmentDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.PutAsJsonAsync($"/api/Department/PutDepartment/{managementDepartmentDto.Id}", managementDepartmentDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var informationData = dataString.FromJson<GetDepartmentGeneralResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (informationData != null && informationData.response != null)
                {
                    return informationData;
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

        public async Task<GetDepartmentGeneralResponse> DeleteDepartment(Guid id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.DeleteAsync($"/api/Department/DeleteDepartment/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var informationData = dataString.FromJson<GetDepartmentGeneralResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (informationData != null && informationData.response != null)
                {
                    return informationData;
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

        public async Task<GetDepartmentGeneralResponse> GetDeparmentById(string id)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/Department/GetDeparment/{id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var informationData = dataString.FromJson<GetDepartmentGeneralResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (informationData != null && informationData.response != null)
                {
                    return informationData;
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
        #region users
        public async Task<GetUserBasicResponse> GetUsersForAssign()
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                var input = new ManagementDepartmentDto();
                var response = await HttpClient.PostAsJsonAsync($"/api/User/GetUsersForAssign", input);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetUserBasicResponse>();
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

        public async Task<GetUserBasicResponse> GetUsersForAssignForEdit(DepartmentIdInputDto departmentIdInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                var response = await HttpClient.PostAsJsonAsync($"/api/User/GetUsersForAssignForEdit", departmentIdInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetUserBasicResponse>();
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

        public async Task<GetDepartmentUsersResponseGeneral> GetDeparmentWithUsers(DepartmentIdInputDto departmentIdInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                var response = await HttpClient.GetAsync($"/api/Department/GetDeparmentWithUsers/{departmentIdInputDto.Id}");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetDepartmentUsersResponseGeneral>();
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

        public async Task<GetResponseAssignUsersGeneral> PostAssingUsersDepartment(AssingUserDepartmentInputDto assingUserDepartmentInputDto)
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);
                var response = await HttpClient.PostAsJsonAsync($"/api/Department/PostAssingUsersDepartment", assingUserDepartmentInputDto);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetResponseAssignUsersGeneral>();
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

        public async Task<GetDepartmentGeneralResponse> PostMassiveDepartament(List<DepartmenstExcelDto> listHandleDeparamentConfig)
        {

            try
            {
                var response = await HttpClient.PostAsJsonAsync($"/api/Department/PostMassiveDepartament", listHandleDeparamentConfig);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<GetDepartmentGeneralResponse>();


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







        #endregion


        #region assign users to worktask

        public async Task<GetDeparmentsWithUsersForWorkTaskResponse> GetDeparmentsWithUsers()
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/Department/GetDeparmentsWithUsers");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var informationData = dataString.FromJson<GetDeparmentsWithUsersForWorkTaskResponse>();
                //var data = await response.Content.ReadFromJsonAsync<GetUserFilterResponse>();


                if (informationData != null && informationData.response != null)
                {
                    return informationData;
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

        public async Task<GetDepartmentListResponse> GetAllDepartments()
        {
            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var response = await HttpClient.GetAsync($"/api/Department/GetAllDepartments");

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                    //return GetDefaultErrorMessage<GetUserFilterResponse>("Error al consultar no tienes suficientes permisos");
                }
                var dataString = await response.Content.ReadAsStringAsync();
                var listOfInstances = dataString.FromJson<GetDepartmentListResponse>();
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

        #endregion
    }
}
