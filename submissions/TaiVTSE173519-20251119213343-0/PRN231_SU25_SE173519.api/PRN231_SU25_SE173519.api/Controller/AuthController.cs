using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models;

namespace PRN231_SU25_SE173519.api.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var result = await _authService.Login(model);
            if (result == null)
                return BadRequest("Email or password is incorrect");
            return Ok(result);
        }
    }
}
