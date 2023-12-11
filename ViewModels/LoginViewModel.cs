using System.ComponentModel.DataAnnotations;

namespace BookingTourWebApp_MVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng điền tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
