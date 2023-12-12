using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTourWebApp_MVC.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<UserTour>? UserTours { get; set; }
    }
}
