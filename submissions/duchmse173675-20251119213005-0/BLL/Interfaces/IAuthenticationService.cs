using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> LoginWithEmailPasswordAsync(string email, string password);
    }
}
