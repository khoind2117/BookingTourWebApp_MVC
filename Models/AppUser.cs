using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTourWebApp_MVC.Models
{
    public class AppUser : IdentityUser
    {
        [ForeignKey("Ticket")]
        public int? TicketId { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public AppUser() 
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
