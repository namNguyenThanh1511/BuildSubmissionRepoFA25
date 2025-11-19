using Microsoft.AspNetCore.Mvc;
using Services.Models.Requests.Auth;
using Services.Services;

namespace PRN231_SU25_SESE181589.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;

        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errorCode = "HB40001", message = GetModelStateErrors() });

            var response = await _authServices.LoginAsync(request);
            
            if (response == null)
                return Unauthorized(new { errorCode = "HB40101", message = "Invalid email or password" });

            return Ok(response);
        }

        private string GetModelStateErrors()
        {
            return string.Join("; ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
        }
    }
}
