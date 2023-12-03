using BookingTourWebApp_MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class BookFlightViewModel
    {
        public AppUser appUser { get; set; }
        public Flight flight { get; set; }
        public int BusinessTickets { get; set; }
        public int EconomyTickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
