using BusinessLogic.Interface;
using BusinessLogic.ModalViews;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173667.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return Unauthorized(new { errorCode = "HB40001", message = "Missing/invalid input." });
            }

            return Ok(result);
        }
    }
}
