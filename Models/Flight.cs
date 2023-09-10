using BookingTourWebApp_MVC.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTourWebApp_MVC.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        public string? LocationFrom { get; set; }
        public string? LocationTo { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime BoardingTime { get; set; }    
        public int ReservedBusinessClassSeat { get; set; } = 0;
        public int ReservedEconomyClassSeat { get; set; } = 0;
        public int Price { get; set; }
        public Pilot? Pilot { get; set; }

        [ForeignKey("Plane")]
        public int? PlaneId { get; set; }
        public Plane? Plane { get; set; }
        
    }
}
