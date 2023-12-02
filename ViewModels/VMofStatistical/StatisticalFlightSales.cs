using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels.VMofStatistical
{
    public class StatisticalFlightSales
    {
        [Key]
        [Display(Name = "ID chuyến bay")]
        public int flightId { get; set; }
        [DisplayName("Số lượng vé BSN")]
        public int businessTickets { get; set; }
        [DisplayName("Đơn giá vé BSN")]
        public decimal businessPrice { get; set; }
        [DisplayName("Số lượng vé ECO")]
        public int economyTickets { get; set; }
        [DisplayName("Đơn giá vé ECO")]
        public decimal economyPrice { get; set; }
        [DisplayName("Tổng doanh thu chuyến bay")]
        public decimal Sales { get; set; }
    }
}
