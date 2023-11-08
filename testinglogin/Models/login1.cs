using System.ComponentModel.DataAnnotations;

namespace testinglogin.Models
{
    public class login1
    {
        
        [Required(ErrorMessage = "Username is required")]

        public string? Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }=string.Empty;
    }
}
