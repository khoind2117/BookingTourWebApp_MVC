using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels.VMofStatistical
{
    public class StatisticalUser
    {
        [Key]
        [DisplayName("Id người dùng")]
        public string Id { get; set; }
        [DisplayName("Tên đầy đủ")]
        public string FullName { get; set; }
        [DisplayName("Số lượng vé BSN")]
        public int businessTickets { get; set; }
        [DisplayName("Số lượng vé ECO")]
        public int economyTickets { get; set; }
        [DisplayName("Tổng số lượng vé")]
        public int TotalTickets { get; set; }
    }
}
