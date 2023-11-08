using System.ComponentModel.DataAnnotations;

namespace testinglogin.Models
{
    public class Registration
    {

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;



        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; } = string.Empty;



        [Compare("password", ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Confirm_password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string phone { get; set; } = string.Empty;
    }
}

