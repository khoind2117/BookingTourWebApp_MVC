using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BookingTourWebApp_MVC.ViewModels.VMofStatistical
{
    public class StatisticalBookingTime
    {
        [Key]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Gmail { get; set; }
        public string phoneNumber { get; set; }
        public DateTime bookingTime { get; set; }
    }
}
