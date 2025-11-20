using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_SU25_SE170497.API.Extensions;

namespace PRN231.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILeopardAccountService _service;

        public AuthController(ILeopardAccountService service)
        {
            _service = service;
        }
        public sealed record LoginRequest(string Email, string Password);

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _service.LoginAsync(request.Email, request.Password);
            return result.ToActionResult();
        }
    }
}
