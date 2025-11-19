using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE180371.api.Controllers;

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
            var firstError = ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Missing/invalid input";

            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = firstError
            });
        }

        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new ErrorResponse
            {
                ErrorCode = "HB40101",
                Message = "Token missing/invalid"
            });
        }

        return Ok(response);
    }
}