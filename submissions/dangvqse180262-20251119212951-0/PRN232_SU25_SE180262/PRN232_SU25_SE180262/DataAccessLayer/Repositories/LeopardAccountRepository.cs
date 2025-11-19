using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class LeopardAccountRepository : ILeopardAccountRepository
    {
        private readonly Su25leopardDbContext _ctx;
        public LeopardAccountRepository(Su25leopardDbContext ctx) => _ctx = ctx;

        public async Task<LeopardAccount?> LoginAsync(string email, string password)
        {
            return await _ctx.LeopardAccounts
                             .FirstOrDefaultAsync(a => a.Email == email
                                                    && a.Password == password);
        }

    }
}
