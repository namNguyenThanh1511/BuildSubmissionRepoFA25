using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LeopardAccountRepository : GenericRepository<LeopardAccount>, ILeopardAccountRepository
    {

        public LeopardAccountRepository(Su25leopardDbContext context) : base(context)
        {
        }

        public async Task<LeopardAccount?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
