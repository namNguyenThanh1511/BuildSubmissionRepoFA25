using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Su25leopardDbContext _context;
        public AccountRepository(Su25leopardDbContext context)
        {
            _context = context;
        }
        public async Task<LeopardAccount> GetByEmailPasswordAsync(string email, string password)
        {
            return await _context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
