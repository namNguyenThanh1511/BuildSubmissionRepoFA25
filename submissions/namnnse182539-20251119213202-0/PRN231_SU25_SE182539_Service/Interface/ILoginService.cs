using Repository.DTO;

namespace Service.Interface
{
    public interface ILoginService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
