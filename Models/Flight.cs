using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTourWebApp_MVC.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public int BusinessCapacity { get; set; }
        public int EconomyCapacity { get; set; }
        public DateTime DepartureTime { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal EconomyPrice { get; set; }
        public DateTime UploadTime { get; set; }

        public int PlaneId { get; set; }
        public virtual Plane? Plane { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
