using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leopard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "Email and password are required"
                });
            }

            var loginResult = await _accountService.LoginAsync(request);
            if (loginResult == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Invalid credentials or unauthorized role"
                });
            }

            return Ok(loginResult);
        }
    }
}
