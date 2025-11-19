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

        public async Task<List<GroupByDTO>> Search(string? leopardName, string? weight)
        {

            var items = await _context.LeopardProfiles
                .Include(x => x.LeopardType)
                .Where(i =>
                    (string.IsNullOrEmpty(leopardName) || i.LeopardName.Contains(leopardName)) &&
                    (string.IsNullOrEmpty(weight) || i.Weight.ToString().Contains(weight)))
                .ToListAsync();

            var grouped = items
                .GroupBy(i => i.LeopardType.LeopardTypeName)
                .OrderBy(g => g.Key)
                .Select(g => new GroupByDTO
                {
                    LeopardType = g.Key,
                    LeopardProfiles = g.OrderByDescending(x => x.LeopardName).ToList()
                })
                .ToList();

            return grouped;
        }

        public async Task<int> GetNextLeopardProfileId()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
            return maxId + 1;
        }


    }
}
