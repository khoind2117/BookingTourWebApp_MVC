namespace BookingTourWebApp_MVC.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
