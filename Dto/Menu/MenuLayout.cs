namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Menu
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Child
    //{
    //    public string title { get; set; }
    //    public string href { get; set; }
    //    public List<Child> children { get; set; }
    //}

    //public class MenuItemLayout
    //{
    //    public string title { get; set; }
    //    public string href { get; set; }
    //    public List<Child> children { get; set; }
    //}

    public class PrincipalMenu
    {
        public List<MenuItemLayout> menu { get; set; }
    }

    public class MenuItemLayout
    {
        public string title { get; set; }
        public string href { get; set; }
        public List<MenuItemLayout> children { get; set; } = new List<MenuItemLayout>();
    }
}
