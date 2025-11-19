using BusinessLogic.DTOs;
using BusinessLogic.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE161136.Controllers
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
        public async Task<ActionResult<LoginResponse>> Login([FromBody] BusinessLogic.DTOs.LoginRequest request)
        {
            Console.WriteLine($"🔐 Login attempt for: {request.Email}");

            try
            {
                var response = await _authService.LoginAsync(request);

                if (response == null)
                {
                    Console.WriteLine($"❌ Login failed for: {request.Email}");
                    return Unauthorized("Invalid email or password");
                }

                Console.WriteLine($"✅ Login successful for: {request.Email} (Role: {response.Role})");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Login error for {request.Email}: {ex.Message}");
                return StatusCode(500, new { error = "Login failed" });
            }
        }
    }
}
