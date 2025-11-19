using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PRN232_SU25_SE180262.api.Controllers 
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILeopardAccountService _accountService;

        public AuthController(ILeopardAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "Email and password are required"
                });
            }

            var result = await _accountService.AuthenticateAsync(request.Email, request.Password);
            if (result == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Invalid email or password"
                });
            }

            return Ok(new AuthResponse
            {
                Token = result.Token,
                Role = result.Role
            });
        }
    }
}
