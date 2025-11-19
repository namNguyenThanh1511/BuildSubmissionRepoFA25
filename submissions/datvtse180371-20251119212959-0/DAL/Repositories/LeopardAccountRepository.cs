using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class LeopardAccountRepository : GenericRepository<LeopardAccount>, ILeopardAccountRepository
{
    public LeopardAccountRepository(Su25leopardDbContext context) : base(context)
    {
    }

    public async Task<LeopardAccount?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var account = await GetByEmailAsync(email);
        return account != null && account.Password == password;
    }
}