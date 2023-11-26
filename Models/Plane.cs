namespace BookingTourWebApp_MVC.Models
{
    public class Plane
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Flight>? Flights { get; set; }
    }
}
