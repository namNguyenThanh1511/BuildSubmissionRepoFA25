using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173362.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.DAL
{
    public class LeopardProfileRepository : GenericRepository<Model.LeopardProfile>
    {
        private readonly Su25leopardDbContext _context;
        public LeopardProfileRepository() => _context ??= new Su25leopardDbContext();

        public async Task<List<Model.LeopardProfile>> Getleopards()
        {
            return await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .ToListAsync();
        }

        public async Task<Model.LeopardProfile> GetleopardById(int id)
        {
            return await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .SingleOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<int> GetMaxIdAsync()
        {
            var maxId = await _context.LeopardProfiles.MaxAsync(h => (int?)h.LeopardProfileId);
            return maxId ?? 0;
        }

    }
}
