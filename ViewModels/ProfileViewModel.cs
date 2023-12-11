using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên là trường bắt buộc.")]
        public string FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
