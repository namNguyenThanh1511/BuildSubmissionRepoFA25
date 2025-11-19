using PRN231_SU25_SE173175.Repository.Entities;

namespace PRN231_SU25_SE173175.Service.Interfaces
{
	public interface ILeopardAccountService
	{
		Task<IEnumerable<LeopardAccount>> GetAllAccountsAsync();
		Task<LeopardAccount?> GetAccountByIdAsync(int accountId);
		Task<LeopardAccount?> GetAccountByUsernameAsync(string username);
		Task<LeopardAccount?> GetAccountByEmailAsync(string email);
		Task<LeopardAccount> CreateAccountAsync(LeopardAccount account);
		Task UpdateAccountAsync(LeopardAccount account);
		Task DeleteAccountAsync(int accountId);
		Task<bool> AccountExistsAsync(int accountId);
		Task<bool> IsUsernameUniqueAsync(string username);
		Task<bool> IsEmailUniqueAsync(string email);
		Task ToggleAccountStatusAsync(int accountId);
		Task UpdateAccountRoleAsync(int accountId, int newRole);
	}
}
