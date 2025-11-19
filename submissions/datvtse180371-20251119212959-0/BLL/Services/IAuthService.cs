using BLL.DTOs;

namespace BLL.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    string GetRoleName(int roleId);
}