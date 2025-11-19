using Repositories.Models;
using Repositories.DTOs;

namespace Repositories.Interfaces
{
    public interface ILeopardAccountRepository
    {
        Task<LeopardAccount?> GetAccountByEmailAsync(string email);
        Task<LeopardAccount?> GetAccountByIdAsync(int accountId);
        Task<IEnumerable<LeopardAccount>> GetAllAccountsAsync();
        Task<LeopardAccount> CreateAccountAsync(LeopardAccount account);
        Task<LeopardAccount> UpdateAccountAsync(LeopardAccount account);
        Task<bool> DeleteAccountAsync(int accountId);
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }
}