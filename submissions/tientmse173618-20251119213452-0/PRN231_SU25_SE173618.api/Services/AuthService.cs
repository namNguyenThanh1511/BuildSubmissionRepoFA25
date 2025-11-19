using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173618.api.Models;
using PRN231_SU25_SE173618.api.Models.DTOs;

namespace PRN231_SU25_SE173618.api.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    bool HasPermission(int roleId, string operation);
}

public class AuthService : IAuthService
{
    private readonly Su25leopardDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(Su25leopardDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var account = await _context.LeopardAccounts
            .FirstOrDefaultAsync(a => a.Email == request.Email && a.Password == request.Password);

        if (account == null)
            return null;

        if (!IsValidRole(account.RoleId))
            return null;

        var token = _jwtService.GenerateToken(account);
        return new LoginResponse
        {
            Token = token,
            Role = account.RoleId.ToString()
        };
    }

    public bool HasPermission(int roleId, string operation)
    {
        return operation switch
        {
            "read" => IsValidRole(roleId),
            "create" => roleId == 5 || roleId == 6,
            "update" => roleId == 5 || roleId == 6,
            "delete" => roleId == 5 || roleId == 6,
            _ => false
        };
    }

    private static bool IsValidRole(int roleId)
    {
        return roleId == 4 || roleId == 5 || roleId == 6 || roleId == 7;
    }
} 