using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE184736.api.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
