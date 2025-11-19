using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Model;

namespace PRN231_SU25_SE._173563api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            if (response == null)
            {
                return Unauthorized(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }
            return Ok(response);
        }
    }
}
