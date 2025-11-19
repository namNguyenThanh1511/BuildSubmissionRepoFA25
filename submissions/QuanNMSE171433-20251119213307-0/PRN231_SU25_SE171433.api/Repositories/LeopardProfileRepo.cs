using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<int> GetNextLeopardProfileId()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId) ?? 0;
            return maxId;
        }
    }
}
