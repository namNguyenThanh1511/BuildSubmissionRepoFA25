using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class LeopardAccountController : ControllerBase
    {
        private readonly LeopardAccountService _authService;
        private readonly IConfiguration _configuration;

        public LeopardAccountController(IConfiguration configuration)
        {
            _authService = new LeopardAccountService();
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
                Role = GetRoleName((int)user.RoleId)
            });
        }

        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "admin",
                2 => "manager",
                3 => "staff",
                4 => "member",
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
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
