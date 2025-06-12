namespace CureFusion.Domain.Entities
{
    
    public class Hospital
    {

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Zone { get; set; } = string.Empty;
        //  public int PhoneNumber { get; set; }
        // public double? Rating { get; set; }
        // public string Website { get; set; } = string.Empty;
        public double? Distance { get; set; }
        public Location Location { get; set; } = null!;
    }
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
