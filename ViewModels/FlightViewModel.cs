using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class FlightViewModel
    {
        [Display(Name = "Mã chuyến bay")]
        public int Id { get; set; }
        //public string GenerateFlightCode(string departure, string destination, DateTime departureTime)
        //{
        //    // Example: Code = DAD-HAN-202301011200 (Departure-Destination-DateTime)
        //    string code = $"{departure.ToUpper()}-{destination.ToUpper()}-{departureTime:yyyyMMddHHmm}";
        //    return code;
        //}
        [Display(Name = "Mã máy bay")]
        public int PlaneId { get; set; }
        [Display(Name = "Tên máy bay")]
        public string PlaneName { get; set; }
        [Display(Name = "Nơi xuất phát")]
        public string Departure { get; set; }
        [Display(Name = "Nơi đến")]
        public string Destination { get; set; }
        [Display(Name = "Số lượng ghế BSN còn")]
        public int BusinessCapacity { get; set; }
        [Display(Name = "Số lượng ghế ECO còn")]
        public int EconomyCapacity { get; set; }
        [Display(Name = "Thời gian bay")]
        public DateTime DepartureTime { get; set; }
        [Display(Name = "Giá vé BSN")]
        public decimal BusinessPrice { get; set; }
        [Display(Name = "Giá vé ECO")]
        public decimal EconomyPrice { get; set; }
        [Display(Name = "Thời điểm Upload")]
        public DateTime UploadTime { get; set; }
        
    }
}
