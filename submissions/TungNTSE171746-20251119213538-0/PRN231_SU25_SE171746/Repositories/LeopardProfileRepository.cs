using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardProfileRepository : GenericRepository<LeopardProfile>
    {
        public LeopardProfileRepository() { }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(x => x.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == id);
        }

        public async Task<List<LeopardProfile>> SearchAsync(string? LeopardName, double? Weight)
        {
            LeopardName = LeopardName?.Trim();

            var query = _context.LeopardProfiles.Include(x => x.LeopardType).AsQueryable();

            if (!string.IsNullOrEmpty(LeopardName))
            {
                query = query.Where(x => x.LeopardName.Contains(LeopardName));
            }

            if (Weight != null)
            {
                query = query.Where(x => x.Weight == Weight);
            }

            return await query.ToListAsync();
        }
        public IQueryable<LeopardProfile> GetQueryable()
        {
            return _context.LeopardProfiles.Include(h => h.LeopardType);
        }

    }
}