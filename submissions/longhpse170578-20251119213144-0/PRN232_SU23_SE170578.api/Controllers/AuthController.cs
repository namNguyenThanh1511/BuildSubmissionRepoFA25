using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232_SU23_SE170578.api.DTO;
using PRN232_SU23_SE170578.api.Services;

namespace PRN232_SU25_SE170578.api.Controllers
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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.Authenticate(request.email, request.password);
                if (result == null)
                {
                    return Unauthorized(new { errorCode = "AUTH401", message = "Invalid credentials" });
                }
                return Ok(new
                {
                    token = result.Value.Token,
                    role = result.Value.Role
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
