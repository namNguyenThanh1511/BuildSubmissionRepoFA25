using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class SubTableRepository : ISubTableRepository
    {
        private readonly LeopardDbContext _context;

        public SubTableRepository(LeopardDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LeopardTypes.AnyAsync(b => b.LeopardTypeId == id);
        }
    }
}
