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
    public class LeopardProfileRepo : GenericRepository<LeopardProfile>
    {
        public LeopardProfileRepo() { }

        public IQueryable<LeopardProfile> GetAllQueryable()
        {
            return _context.LeopardProfiles.Include(x => x.LeopardType);
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            var items = await _context.LeopardProfiles.Include(x => x.LeopardType).ToListAsync();

            return items;
        }

        public async Task<LeopardProfile> GetByIdAsync(int code)
        {
            var item = await _context.LeopardProfiles.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == code);

            return item;
        }

        public async Task<List<LeopardProfile>> Search(string? leopardName, double? weight)
        {

            var items = await _context.LeopardProfiles
                .Include(x => x.LeopardType)
                .Where(i =>
                    (string.IsNullOrEmpty(leopardName) || i.LeopardName.Contains(leopardName)) &&
                    (weight == null || i.Weight == weight))
                .ToListAsync();

            return items;
        }

        public async Task<int> GetLeopardId()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
            return maxId;
        }
    }
}
