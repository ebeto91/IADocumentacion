using RAS823_MC_CiudadMunicipal_FrontEnd.Dto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class GetUserFilterResponse
    {
        public ResultModel response { get; set; }
        public GetUserFilterResponseDefinition definition { get; set; }
    }

    public class GetUserFilterLogin
    {
        public ResultModel response { get; set; }
        public UserToken definition { get; set; }
    }

    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime PwdExpiredDate { get; set; }
    }

    public partial class GetUserFilterResponseDefinition
    {
        public int totalCount { get; set; }
        public List<UserResponse> items { get; set; }
    }

    #region listado 
    public class GetUserForListFilterResponse
    {
        public ResultModel response { get; set; }
        public GetUserForListFilterResponseDefinition definition { get; set; }
    }


    public partial class GetUserForListFilterResponseDefinition
    {
        public int totalCount { get; set; }
        public List<GetUserForList> items { get; set; }
    }

    #endregion

    #region users basic
    public class GetUserBasicResponse
    {
        public ResultModel response { get; set; }
        public List<UserResponseAssing> definition { get; set; }
    }
    public partial class GetUserBasicListDefinition
    {
        public List<UserResponse> items { get; set; }
    }

    #endregion
}
