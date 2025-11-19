using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170516.api.DTOs;
using Services.DTOs;
using Services.Interfaces;

namespace PRN231_SU25_SE170516.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAuthService _authService;
        public authController(IAuthService authService)
        {

            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_INPUT,
                        Message = "Invalid input data"
                    });
                }

                (bool success, string token, int role) = await _authService.LoginAsync(request);

                if (!success)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Invalid credentials or unauthorized role"
                    });
                }

                return Ok(new LoginResponse
                {
                    Token = token,
                    Role = role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCodes.INTERNAL_ERROR,
                    Message = "Internal server error"
                });
            }
        }
    }
}
