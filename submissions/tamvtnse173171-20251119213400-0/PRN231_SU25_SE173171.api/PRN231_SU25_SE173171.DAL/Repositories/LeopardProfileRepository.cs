using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173171.DAL.Entities;
using PRN231_SU25_SE173171.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PRN231_SU25_SE173171.DAL.Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            return await _context.LeopardProfiles.Include(s => s.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            return await _context.LeopardProfiles.Include(s => s.LeopardType)
                                          .FirstOrDefaultAsync(s => s.LeopardProfileId == id);
        }

        public async Task Add(LeopardProfile profile)
        {
            _context.LeopardProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task Update(LeopardProfile profile)
        {
            _context.LeopardProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var profile = await _context.LeopardProfiles.FindAsync(id);
            if (profile != null)
            {
                _context.LeopardProfiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
