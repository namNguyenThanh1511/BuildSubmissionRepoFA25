using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170533.api.DTOs;
using BLL.DTOs;
using BLL.Interface;

namespace PRN231_SU25_SE170533.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public authController(IAccountService accountService)
        {
            _accountService = accountService;
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

                (bool success, string token, string role) = await _accountService.LoginAsync(request);

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
