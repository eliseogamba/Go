namespace Go.Models
{
    public class Location
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Province Province { get; set; }
    }
}