using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class CreateTourViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn điểm khởi hành.")]
        public string Departure { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn điểm đến.")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày đi.")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày trở về.")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá tiền.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình ảnh.")]
        public IFormFile Image { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
