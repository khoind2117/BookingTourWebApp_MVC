using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class StatisticalView
    {
        [Key]
        public int Id { get; set; }
        public List<StatisticalFlightSales> HighSales { get; set; }
        public List<StatisticalFlightSales> LowSales { get;set; }
        public List<StatisticalFlightSales> AllSaleInMonth { get; set; }
        public StatisticalTotalSaleMonth TotalSaleInMonth { get; set; }
        public List<StatisticalUser> UserHaveHighTicket { get; set; }
        public List<StatisticalUser> UserHaveLowTicket { get; set; }
    }
}
