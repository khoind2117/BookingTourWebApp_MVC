using BookingTourWebApp_MVC.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class FlightViewModel
    {
        //[Display(Name = "Mã chuyến bay")]
        //public int Id { get; set; }
        //[Display(Name = "Mã máy bay")]
        //public int PlaneId { get; set; }
        //[Display(Name = "Tên máy bay")]
        //public string PlaneName { get; set; }
        //[Display(Name = "Nơi xuất phát")]
        //public string Departure { get; set; }
        //[Display(Name = "Nơi đến")]
        //public string Destination { get; set; }
        //[Display(Name = "Số lượng ghế BSN còn")]
        //public int BusinessCapacity { get; set; }
        //[Display(Name = "Số lượng ghế ECO còn")]
        //public int EconomyCapacity { get; set; }
        //[Display(Name = "Thời gian bay")]
        //public DateTime DepartureTime { get; set; }
        //[Display(Name = "Giá vé BSN")]
        //public decimal BusinessPrice { get; set; }
        //[Display(Name = "Giá vé ECO")]
        //public decimal EconomyPrice { get; set; }
        //[Display(Name = "Thời điểm Upload")]
        //public DateTime UploadTime { get; set; }
        //public IEnumerable<Plane>? Planes { get; set; }


        [Display(Name = "Mã chuyến bay")]
        public int Id { get; set; }
        [Display(Name = "Mã máy bay")]

        public int PlaneId { get; set; }

        [Display(Name = "Tên máy bay")]
        [BindNever]
        [ValidateNever]
        public string PlaneName { get; set; }

        [Display(Name = "Nơi xuất phát")]
        [Required(ErrorMessage = "Vui lòng nhập nơi xuất phát.")]
        public string Departure { get; set; }

        [Display(Name = "Nơi đến")]
        [Required(ErrorMessage = "Vui lòng nhập nơi đến.")]
        public string Destination { get; set; }

        [Display(Name = "Số lượng ghế BSN còn")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng ghế Business.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng ghế Business không hợp lệ.")]
        public int BusinessCapacity { get; set; }

        [Display(Name = "Số lượng ghế ECO còn")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng ghế Economy.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng ghế Economy không hợp lệ.")]
        public int EconomyCapacity { get; set; }

        [Display(Name = "Thời gian bay")]
        [Required(ErrorMessage = "Vui lòng nhập thời gian bay.")]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "Giá vé BSN")]
        [Required(ErrorMessage = "Vui lòng nhập giá vé Business.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá vé Business không hợp lệ.")]
        public decimal BusinessPrice { get; set; }

        [Display(Name = "Giá vé ECO")]
        [Required(ErrorMessage = "Vui lòng nhập giá vé Economy.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá vé Economy không hợp lệ.")]
        public decimal EconomyPrice { get; set; }

        [Display(Name = "Thời điểm Upload")]
        [ValidateNever]
        public DateTime UploadTime { get; set; }

        //public IEnumerable<Plane>? Planes { get; set; }
    }
}
