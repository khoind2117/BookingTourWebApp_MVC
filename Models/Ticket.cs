using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTourWebApp_MVC.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string? SeatClass { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set;}
        [ForeignKey("Flight")]
        public int? FlightId { get; set; }
        public Flight? Flight { get; set; }
    }
}
