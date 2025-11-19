using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE184278.services;
using PRN231_SU25_SE184278.services.DTOs;

namespace PRN231_SU25_SE184278.api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "Email and password are required"
                });
            }

            var loginResult = await _service.LoginAsync(request);
            if (loginResult == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Invalid credentials or unauthorized role"
                });
            }

            return Ok(loginResult);
        }
    }
}
