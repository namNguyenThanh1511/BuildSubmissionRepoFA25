using BusinessLogicLayer.Services;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE170076.api.Controllers
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


        //API Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Missing or invalid input");

            var user = await _authService.Login(model);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = await _authService.GenerateJwtToken(user.UserName);

            return Ok(token);
        }
    }
}
