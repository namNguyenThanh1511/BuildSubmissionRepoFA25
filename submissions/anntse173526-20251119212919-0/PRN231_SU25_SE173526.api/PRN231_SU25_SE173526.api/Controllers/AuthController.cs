using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173526.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var response = _authService.Login(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = ex.Message
                });
            }
        }
    }
}
