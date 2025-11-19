using Repositories.DTOs;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequest);
        Task<bool> ValidateTokenAsync(string token);
        Task<string?> GetUserRoleAsync(string email);
    }
}