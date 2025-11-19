using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE184437.API.ModelExtensions;
using PRN231_SU25_SE184437.BLL.Services;

namespace PRN231_SU25_SE184437.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly LeopardAccountService _service;

        public AuthController(LeopardAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _service.LoginAsync(request.Email, request.Password);
            if (response == null)
            {
                var errorResponse = new PRN231_SU25_SE184437.DAL.ModelExtensions.ApiResponse<object>
                {
                    Success = false,
                    Data = null,
                    DetailError = new PRN231_SU25_SE184437.DAL.ModelExtensions.DetailError
                    {
                        ErrorCode = "HB40101",
                        Message = "Invalid username or password"
                    }
                };
                return Unauthorized(errorResponse);
            }
            return Ok(response);
        }
    }
}
