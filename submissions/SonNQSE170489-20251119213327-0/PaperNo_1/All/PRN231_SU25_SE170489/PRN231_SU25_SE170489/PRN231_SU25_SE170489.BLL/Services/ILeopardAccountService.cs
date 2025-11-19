using PRN231_SU25_SE170489.DAL.ModelExtensions;

namespace PRN231_SU25_SE170489.BLL.Services
{
	public interface ILeopardAccountService
	{
		Task<Result<TokenResponse>> LoginAsync(string email, string password);
	}
}
