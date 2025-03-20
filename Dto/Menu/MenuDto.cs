using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Menu
{
    public class GetMenuLayoutResponse
    {
        public ResultModel response { get; set; }
        public string definition { get; set; }
    }
    public class GetMenuFilterResponse
    {
        public ResultModel response { get; set; }
        public List<MenuDto> definition { get; set; }
    }

    public class MenuDto
    {

        public int MenuModuleId { get; set; }
        public string ModuleKeyName { get; set; }
        public string ModuleUrl { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool IsDefault { get; set; }
        public bool IsVisible { get; set; }
        public string MenuFileName { get; set; }
        public List<MenuModuleItemDto> ListMenuDto { get; set; }
    }
        public class MenuModuleItemDto
        {
            public Guid Id { get; set; }
            public int MenuModuleId { get; set; }
            public string MenuModuleItemKeyName { get; set; }
            public string MenuModuleItemUrl { get; set; }
            //public string MenuFileName { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
            public int Order { get; set; }
            public bool IsVisible { get; set; }
            public bool Enabled { get; set; }
        }
    }

