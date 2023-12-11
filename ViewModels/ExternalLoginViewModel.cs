using System.ComponentModel.DataAnnotations;

namespace FlightBooking.ViewModel
{
    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng điền họ và tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng điền email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
