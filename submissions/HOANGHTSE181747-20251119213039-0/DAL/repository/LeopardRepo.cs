using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repository
{
    public class LeopardRepo : ILeopardRepo
    {
        private readonly Su25leopardDbContext _context = new Su25leopardDbContext();


        public async Task AddLeopardProfile(LeopardProfile LeopardProfile)
        {
            _context.Set<LeopardProfile>().Add(LeopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeopardProfile(string id)
        {
            LeopardProfile profile = await _context.Set<LeopardProfile>().FirstAsync(m => m.LeopardProfileId == int.Parse(id));

            _context.Set<LeopardProfile>().Remove(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<LeopardProfile> GetLeopardProfile(string id)
        {
            return await _context.Set<LeopardProfile>().Include(m => m.LeopardType).FirstAsync(m => m.LeopardProfileId == int.Parse(id));
        }

        public async Task<List<LeopardProfile>> GetLeopardProfiles()
        {
            return await _context.Set<LeopardProfile>().Include(m => m.LeopardType).ToListAsync();
        }

        public async Task UpdateLeopardProfile(LeopardProfile LeopardProfile)
        {
            _context.Set<LeopardProfile>().Update(LeopardProfile);
            await _context.SaveChangesAsync();
        }
    }
}
