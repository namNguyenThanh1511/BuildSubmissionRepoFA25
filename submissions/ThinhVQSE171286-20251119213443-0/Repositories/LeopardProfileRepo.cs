using Microsoft.EntityFrameworkCore;
using Repositories.DTOs;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardProfileRepo : GenericRepository<LeopardProfile>
    {
        public LeopardProfileRepo()
        {
        }
        public IQueryable<LeopardProfile> GetAllQueryable(LeopardProfileFilter filter)
        {
            var query = _context.LeopardProfiles.
                Include(c => c.LeopardType).
                AsQueryable();

            if (!string.IsNullOrEmpty(filter.LeopardName))
                query = query.Where(p => p.LeopardName.Contains(filter.LeopardName));
            if (filter.weight >= 0)
                query = query.Where(p => p.Weight >= filter.weight);
            return query;
        }
        public async Task<LeopardProfile> GetByIdAsync(int code)
        {
            var item = await _context.LeopardProfiles.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == code);
            return item;
        }
    }
}