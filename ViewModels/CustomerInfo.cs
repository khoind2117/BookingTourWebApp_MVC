using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class CustomerInfo
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Không được để trống tên tài khoản")]
        [MinLength(5, ErrorMessage ="Tên tài khoản phải có ít nhất 5 kí tự")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Không được để trống tên đầy đủ của tài khoản này")]
        [MinLength(3, ErrorMessage = "Tên tài khoản phải có ít nhất 5 kí tự")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Không được để trống tên đầy đủ của tài khoản này")]
        [EmailAddress(ErrorMessage = "Định dạng gmail không đúng, Vui lòng nhập lại")]
        public string gmail { get; set; }
        [Required(ErrorMessage = "Không được để trống Số điện thoại của tài khoản này")]
        [RegularExpression("\\d{10}")]
        public string phoneNumber { get; set; }



    }
}
