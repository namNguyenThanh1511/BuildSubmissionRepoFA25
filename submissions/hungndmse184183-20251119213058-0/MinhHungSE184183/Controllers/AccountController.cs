using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs;
using Services.Interface;

namespace MinhHungSE184183.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Repositories.DTOs.LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (result == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Token missing or invalid"
                });
            }

            return Ok(result);
        }
    }
}
