using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE184386.API.ModelExtensions;
using PRN231_SU25_SE184386.BLL.Services;

namespace PRN231_SU25_SE184386.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly LeopardAccountService _service;

        public AuthController(LeopardAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _service.LoginAsync(request.Email, request.Password);
            return Ok(response);
        }
    }
}
