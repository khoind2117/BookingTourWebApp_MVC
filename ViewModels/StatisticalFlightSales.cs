using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class StatisticalFlightSales
    {
        [Key]
        [Display(Name = "Flight ID")]
        public int flightId { get; set; }
        public int businessTickets {  get; set; }
        public decimal businessPrice { get; set; }
        public int economyTickets {  get; set; }
        public decimal economyPrice { get; set; }
        public decimal Sales {  get; set; }
    }
}
