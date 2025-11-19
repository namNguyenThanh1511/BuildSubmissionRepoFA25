using PRN232_SU25_SE170497.DAL.ModelExtensions;

namespace BLL.Services
{
    public interface ILeopardAccountService
    {
        Task<Result<TokenResponse>> LoginAsync(string email, string password);
    }
}
