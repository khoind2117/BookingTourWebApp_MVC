namespace BookingTourWebApp_MVC.Models
{
    public class Plane
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int BusinessClassSeat { get; set; } = 20;
        public int EconomyClassSeat { get; set; } = 50;
    }
}
