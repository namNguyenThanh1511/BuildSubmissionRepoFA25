
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Model
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
