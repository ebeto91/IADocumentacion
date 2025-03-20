using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.ConfigSetting
{
    #region response
    public class GetConfigSettingsResponse
    {
        public ResultModel response { get; set; }
        public List<ConfigSettingModel> definition { get; set; }
    }
    public class ConfigSettingInput
    {
        public string NameFilter { get; set; }
    }

    public class GetConfigSettingsModelResponse
    {
        public ResultModel response { get; set; }
        public string definition { get; set; }
    }
    #endregion
    public class ConfigSettingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Type { get; set; }
        public string Collection { get; set; }
        public string Description { get; set; }
        public string DisplayLabel { get; set; }
    }

    public class ConfigSettingResponse
    {
        public ResultModel Response { get; set; }
        public object Definition { get; set; } = null;
    }

    public enum SettingType
    {
        String,
        Number,
        Decimal,
        Boolean,
        Select
    }
}
