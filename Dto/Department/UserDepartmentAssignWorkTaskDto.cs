using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department
{
    #region response

    public class GetDeparmentsWithUsersForWorkTaskResponse
    {
        public ResultModel response { get; set; }
        public List<UserDepartmentDto> definition { get; set; }
    }

    #endregion
}
