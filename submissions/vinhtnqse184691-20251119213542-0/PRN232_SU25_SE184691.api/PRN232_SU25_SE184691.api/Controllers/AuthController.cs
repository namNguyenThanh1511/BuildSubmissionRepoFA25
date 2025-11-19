using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.DTO;
using Service;

namespace PRN232_SU25_SE184691.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _serv;

        public AuthController(AuthService serv)
        {
            _serv = serv;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginForm form)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, new
                {
                    errorCode = "HB40001",
                    message = "Email or Password is required"
                });
            var result = await _serv.Login(form.email, form.password);

            if (result.code == 404)
                return StatusCode(result.code, new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });

            return StatusCode(result.code, result.item);
        }
    }
}
