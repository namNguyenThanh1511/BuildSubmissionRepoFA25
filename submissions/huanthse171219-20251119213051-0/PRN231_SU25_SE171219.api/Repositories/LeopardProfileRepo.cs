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
            return _context.LeopardProfiles.Include(x => x.LeopardType).AsQueryable();
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

        public async Task<List<LeopardProfile>> Search(string? leopardName)
        {
            var items = await _context.LeopardProfiles
                .Include(x => x.LeopardType)
                .Where(i => (i.LeopardName.Contains(leopardName) || string.IsNullOrEmpty(leopardName))
                )
                .OrderByDescending(x => x.LeopardName).ToListAsync();

            return items;
        }
    }
}

