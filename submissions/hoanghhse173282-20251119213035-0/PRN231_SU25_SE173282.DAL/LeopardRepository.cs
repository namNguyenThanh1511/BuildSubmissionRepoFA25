using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173282.DAL.Model;

namespace PRN231_SU25_SE173282.DAL
{
    public class LeopardRepository : GenericRepository<LeopardProfile>
    {
        private readonly Su25leopardDbContext _context;
        public LeopardRepository() => _context ??= new Su25leopardDbContext();

        public async Task<List<LeopardProfile>> GetHandbags()
        {
            return await _context.LeopardProfiles
                .Include(h => h.LeopardTypeId)
                .ToListAsync();
        }

        public async Task<LeopardProfile> GetHandbagById(int id)
        {
            return await _context.LeopardProfiles
                .Include(h => h.LeopardTypeId)
                .SingleOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<int> GetMaxIdAsync()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId);
            return maxId ?? 0;
        }

    }
}
