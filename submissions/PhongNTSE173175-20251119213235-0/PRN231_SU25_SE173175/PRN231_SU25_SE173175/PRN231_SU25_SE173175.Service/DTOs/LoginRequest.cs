using System.ComponentModel.DataAnnotations;

namespace PRN231_SU25_SE173175.Service.DTOs
{
	public class LoginRequest
	{
		[Required(ErrorMessage = "Required")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Required")]
		public string Password { get; set; }
	}
}

