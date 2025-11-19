using BLL.DTOs;

namespace BLL.Services
{
    public interface ILeopardAccountService 
    {
        Task<LoginResponse> Login(LoginDTO dto);
    }
}
