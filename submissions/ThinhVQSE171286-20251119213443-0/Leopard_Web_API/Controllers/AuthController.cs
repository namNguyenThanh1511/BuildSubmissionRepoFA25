using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Leopard_Web_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _authService = new AuthService();
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "HB40001",
                    Message = "email and password are required"
                });
            }

            var user = await _authService.Authenticate(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "HB40101",
                    Message = "Invalid email or password"
                });
            }

            // Check valid RoleId
            if (user.RoleId is not (4 or 5 or 6 or 7))
            {
                return StatusCode(403, new ErrorResponse
                {
                    ErrorCode = "HB40301",
                    Message = "Permission denied"
                });
            }

            var token = JwtHelper.GenerateToken(user, _configuration);

            return Ok(new AuthResponse
            {
                Token = token,
                Role = user.RoleId
            });
        }


        public class AuthRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class AuthResponse
        {
            public string Token { get; set; }
            public int Role { get; set; }
        }

        public class ErrorResponse
        {
            public string ErrorCode { get; set; }
            public string Message { get; set; }
        }
    }
}
