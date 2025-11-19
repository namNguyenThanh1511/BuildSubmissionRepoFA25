using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly SU25LeopardDBDbContext _context;

        public LeopardAccountRepository(SU25LeopardDBDbContext context)
        {
            _context = context;
        }

        public async Task<LeopardAccount?> GetAccountByEmailAsync(string email)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<LeopardAccount?> GetAccountByIdAsync(int accountId)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<IEnumerable<LeopardAccount>> GetAllAccountsAsync()
        {
            return await _context.LeopardAccounts.ToListAsync();
        }

        public async Task<LeopardAccount> CreateAccountAsync(LeopardAccount account)
        {
            _context.LeopardAccounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<LeopardAccount> UpdateAccountAsync(LeopardAccount account)
        {
            var existingAccount = await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.AccountId == account.AccountId);

            if (existingAccount != null)
            {
                _context.Entry(existingAccount).CurrentValues.SetValues(account);
                await _context.SaveChangesAsync();
            }

            return account;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account != null)
            {
                _context.LeopardAccounts.Remove(account);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var account = await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email);

            if (account != null)
            {
                // So sánh password trực tiếp
                return password == account.Password;
            }

            return false;
        }
    }
}