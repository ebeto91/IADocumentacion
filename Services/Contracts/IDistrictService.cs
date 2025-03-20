using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using System.Net.Http.Json;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts
{
    public interface IDistrictService
    {
        Task<List<DistrictNeighborhoodsDefinition>> GetDistricts();
        //Task<List<DistrictNeighborhoodsResponse>> GetDistrictsWithNeighborhoods(HandleAssociationConfig handleAssociationConfig);
        Task<List<Catalog>> GetNeighborhoodsByDistrict(HandleAssociationConfig handleAssociationConfig);
    }
    class DistrictService : IDistrictService
    {
        public HttpClient HttpClient { get; }
        public DistrictService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        public async Task<List<DistrictNeighborhoodsDefinition>> GetDistricts()
        {

            try
            {
                //var user = await this.HttpClient.PostAsync<ResponseModel<IEnumerable<UserResponse>>>("api/User/GetUsersFilter", userInputDto);

                var data = new CatalogResponse();

                var response = await HttpClient.PostAsJsonAsync($"/api/District/GetDistrictsWithNeighborhoods", data);

                if (response != null && response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                var contentString = await response.Content.ReadAsStringAsync();
                var responseData = contentString.FromJson<DistrictNeighborhoodsResponse>();


                if (responseData != null && responseData.definition.Count > 0)
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

        //public async Task<List<DistrictNeighborhoodsResponse>> GetDistrictsWithNeighborhoods(HandleAssociationConfig handleAssociationConfig)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<Catalog>> GetNeighborhoodsByDistrict(HandleAssociationConfig handleAssociationConfig)
        {
            throw new NotImplementedException();
        }
    }
}
