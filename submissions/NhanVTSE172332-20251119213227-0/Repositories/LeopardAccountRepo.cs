using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;

namespace Repositories
{
	public class LeopardAccountRepo : GenericRepository<LeopardAccount>
	{
		public LeopardAccountRepo() { }

		public async Task<LeopardAccount> GetUserAccountAsync(string email, string password)
		{
			return await _context.LeopardAccounts.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower())
			&& u.Password.Equals(password));
		}
	}
}
