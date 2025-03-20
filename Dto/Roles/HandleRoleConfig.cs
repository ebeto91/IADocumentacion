using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles
{

    public class MenuModuleRolConfigScopeResponse
    {
        public ResultModel response { get; set; }
        public MenuModuleRolConfigScopeResponseDefinition definition { get; set; }
    }
    public class MenuModuleRolConfigScopeResponseDefinition
    {
        //public List<CategoryScope> ListCategoryScopeToShow { get; set; }
        public List<MenuModuleRolConfigDto> MenuModuleRolConfigDto { get; set; }
    }
    public class MenuModuleRolConfigDto
    {
        public int Id { get; set; }
        public string ModuleKeyName { get; set; }
        public string ModuleUrl { get; set; }
        //public string MenuFileName { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool IsDefault { get; set; }
        public bool IsVisible { get; set; }

        public bool Enabled { get; set; }
        public bool Selected { get; set; }
        /// <summary>
        /// Permite indicar que el modulo solo va a ser seleccionado, no tiene nada desplegable, no items, no permisos
        /// </summary>
        public bool ShowOnlySelectModule { get; set; }
        /// <summary>
        /// Permite indicar que el modulo va a ser seleccionado y mostrar la opción, tiene algo desplegable, no items, si permisos
        /// </summary>
        public bool ShowSelectScopesModule { get; set; }

        public List<MenuModuleItemRolConfigDto> ListItemModules { get; set; }
        //public virtual List<ScopeDto> ListScopes { get; set; }

        public List<CategoryScope> ListCategoryScopes { get; set; }
    }

    public class MenuModuleItemRolConfigDto
    {
        public int Id { get; set; }
        public int MenuModuleId { get; set; }
        public string MenuModuleItemKeyName { get; set; }
        //public string MenuFileName { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; }
        public bool Selected { get; set; }
        public bool IsVisible { get; set; }
        //public string RelatedModuleItems { get; set; }
        //public string RelatedScopesOptions { get; set; }
    }

    public class CategoryScope
    {
        public string CategoryValue { get; set; }
        public string CategoryName { get; set; }
        public string DescriptionCategory { get; set; }
        public bool ShowItem { get; set; }
        public bool Selected { get; set; }
        public List<ScopeDto> ListScopes { get; set; }
    }
    public class ScopeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameTranslate { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }

        public bool Selected { get; set; }

        public Guid RoleScopeId { get; set; }

    }
    #region edit and create
    public class RoleCategoryScopeSave
    {
        public RoleDto role { get; set; }
        public List<MenuModuleRolConfigDto> MenuModuleRolConfigDto { get; set; }
    }

    public class GetRolByIdResponse
    {
        public ResultModel response { get; set; }
        public RoleForEditDto definition { get; set; }
    }

    public class RoleForEditDto : RoleDto
    {
        public List<MenuModuleRolConfigDto> MenuModuleRolConfigDto { get; set; }

    }

    #endregion
}
