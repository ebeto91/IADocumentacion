namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Cords
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Coords
    {
        public double accuracy { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public object altitude { get; set; }
        public object altitudeAccuracy { get; set; }
        public object heading { get; set; }
        public object speed { get; set; }
    }

    public class Coordinates
    {
        public long timestamp { get; set; }
        public Coords coords { get; set; }
    }
}
