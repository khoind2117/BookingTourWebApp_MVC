using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng điền họ và tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng điền email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng điền mật khẩu xác nhận")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
