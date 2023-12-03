namespace BookingTourWebApp_MVC.Models
{
    public class Booking
    {
        public int Id { get; set; } // Khóa chính mới

        public string? AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public int? FlightId { get; set; }
        public virtual Flight? Flight { get; set; }

        public int BusinessTickets { get; set; }
        public int EconomyTickets { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
