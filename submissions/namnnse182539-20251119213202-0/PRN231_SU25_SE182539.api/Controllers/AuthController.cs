using Microsoft.AspNetCore.Mvc;
using Repository.DTO;
using Service.Interface;

namespace PRN231_SU25_SE182539.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _loginService.LoginAsync(request);

            if (response == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Token Missing / Invalid email, password, or role not allowed"
                });
            }

            return Ok(response);
        }
    }
}
