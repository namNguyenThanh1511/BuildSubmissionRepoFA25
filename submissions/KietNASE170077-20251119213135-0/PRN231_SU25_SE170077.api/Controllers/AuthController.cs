using BLL;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE170077.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _authService.Login(model);
            if (user == null)
            {
                return Unauthorized(new { errorCode = "HB40101", message = "Invalid credentials" });
            }

            var token = await _authService.GenerateJwtToken(user.FullName, user.RoleId);

            return Ok(token);
        }
    }
}
