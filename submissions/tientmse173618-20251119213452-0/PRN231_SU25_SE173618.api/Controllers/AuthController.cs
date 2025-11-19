using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE173618.api.Models.DTOs;
using PRN231_SU25_SE173618.api.Services;

namespace PRN231_SU25_SE173618.api.Controllers;

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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = "Invalid input data"
            });
        }

        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new ErrorResponse
            {
                ErrorCode = "HB40101",
                Message = "Invalid credentials or unauthorized role"
            });
        }

        return Ok(response);
    }
} 