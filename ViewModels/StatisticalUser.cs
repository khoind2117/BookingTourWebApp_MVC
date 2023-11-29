using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class StatisticalUser
    {
        [Key]
        public string Id { get; set; }
        public string FullName { get; set; }
        public int businessTickets { get; set; }
        public int economyTickets { get; set; }

        public int TotalTickets { get; set; }
    }
}
