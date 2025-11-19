using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SU25Leopard.BusinessLogicLayer.Services;
using SU25Leopard.BusinessObject.DTO;

namespace SU25Leopard.WebAPI.Controller
{
    [Route("api")]
    [ApiController]
    public class LeopardAccountController : ControllerBase
    {
        private LeopardAccountService _leopardAccountService;
        public LeopardAccountController(LeopardAccountService leopardAccountService)
        {
            _leopardAccountService = leopardAccountService;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                    return BadRequest(ErrorResponse.BadRequest("email is required"));

                if (string.IsNullOrEmpty(request.Password))
                    return BadRequest(ErrorResponse.BadRequest("password is required"));

                var result = await _leopardAccountService.Login(request.Email, request.Password);

                if (result == null)
                    return Unauthorized(ErrorResponse.Unauthorized("Invalid email or password"));

                if (!result.Role.Equals("4") && !result.Role.Equals("5") && !result.Role.Equals("6") && !result.Role.Equals("7"))
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
