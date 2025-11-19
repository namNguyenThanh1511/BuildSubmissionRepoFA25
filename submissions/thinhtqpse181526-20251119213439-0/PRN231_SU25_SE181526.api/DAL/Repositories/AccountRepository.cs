using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountRepository : GenericRepository<LeopardAccount>, IAccountRepository
    {
        public AccountRepository(Su25leopardDbContext context) : base(context)
        {
        }
        public async Task<LeopardAccount?> LoginAsync(string email, string password)
        {
            return await FindAll(a => a.Email == email && a.Password == password).FirstOrDefaultAsync();
        }
    }
}
