using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE184930.bll.Interfaces;
using PRN231_SU25_SE184930.dal.DTOs;
using PRN231_SU25_SE184930.dal.Enums;

namespace PRN231_SU25_SE184930.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.BadRequest,
                    Message = "Invalid input data"
                });
            }

            try
            {
                var result = await _authService.LoginAsync(loginRequest);

                if (result == null)
                {
                    return Unauthorized(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Unauthorized,
                        Message = "Invalid credentials or role not allowed"
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalServerError,
                    Message = "Internal server error"
                });
            }
        }
    }
}
