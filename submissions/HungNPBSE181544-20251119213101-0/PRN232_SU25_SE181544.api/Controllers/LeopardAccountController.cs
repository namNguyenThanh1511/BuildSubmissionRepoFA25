using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232_SU25_SE181544.BLL.DTOs;
using PRN232_SU25_SE181544.BLL.Services;

namespace PRN232_SU25_SE181544.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LeopardAccountController : ControllerBase
    {
        private readonly LeopardAccountService _systemAccountService;
        public LeopardAccountController(LeopardAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                    return BadRequest(ErrorResponse.BadRequest("email is required"));

                if (string.IsNullOrEmpty(request.Password))
                    return BadRequest(ErrorResponse.BadRequest("password is required"));

                var result = await _systemAccountService.Login(request.Email, request.Password);

                if (result == null)
                    return Unauthorized(ErrorResponse.Unauthorized("Invalid email or password"));


                if (result.Role != "5"&& result.Role != "6" && result.Role != "7" && result.Role != "4")
                    return Unauthorized(ErrorResponse.Unauthorized("Role not authorized"));


                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }
    }
}
