using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170489.API.Extensions;
using PRN231_SU25_SE170489.BLL.Services;

namespace PRN231_SU25_SE170489.API.Controllers
{
	[Route("api/auth")]
	[ApiController]
	[AllowAnonymous]
	public class AuthController : ControllerBase
	{
		private readonly ILeopardAccountService _service;

		public AuthController(ILeopardAccountService service)
		{
			_service = service;
		}
		public sealed record LoginRequest(string Email, string Password);

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var result = await _service.LoginAsync(request.Email, request.Password);
			return result.ToActionResult();
		}
	}
}
