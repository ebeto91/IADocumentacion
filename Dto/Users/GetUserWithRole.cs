using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class GetUserWithRole : UserResponse
    {
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
    public class GetUserForList : GetUserWithRole
    {
        public string? DepartmentName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    #region response api
    public class GetUserWithRoleResponse
    {
        public ResultModel response { get; set; }
        public GetUserWithRole definition { get; set; }
    }
    #endregion
}
