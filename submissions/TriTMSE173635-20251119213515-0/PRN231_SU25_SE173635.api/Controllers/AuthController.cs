using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173635.api.Models;
using PRN231_SU25_SE173635.api.Models.DTOs;
using PRN231_SU25_SE173635.api.Services;

namespace PRN231_SU25_SE173635.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Su25leopardDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(Su25leopardDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var account = await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == request.Email && a.Password == request.Password);

            if (account == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.TokenMissing,
                    Message = "Invalid email or password"
                });
            }

            if (account.RoleId != 4 && account.RoleId != 5 && account.RoleId != 6 && account.RoleId != 7)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.TokenMissing,
                    Message = "No token issued for this role"
                });
            }

            var token = _jwtService.GenerateToken(account);

            return Ok(new LoginResponse
            {
                Token = token,
                Role = account.RoleId.ToString()
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }
} 