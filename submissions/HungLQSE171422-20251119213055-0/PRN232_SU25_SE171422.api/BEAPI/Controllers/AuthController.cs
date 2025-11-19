using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repositories.Models;
using Services;
using System.Threading.Tasks;

namespace BEAPI.Controllers
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
            if (user.RoleId is not (5 or 6 or 7 or 4))
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
                Role = GetRoleName((int)user.RoleId)
            });
        }

        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                5 => "5",
                6 => "6",
                7 => "7",
                4 => "4",
                _ => "unknown"
            };
        }

        // Inline models:
        public class AuthRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class AuthResponse
        {
            public string Token { get; set; }
            public string Role { get; set; }
        }

        public class ErrorResponse
        {
            public string ErrorCode { get; set; }
            public string Message { get; set; }
        }
    }
}
