using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LeopardProfileRepository
    {
        private readonly Su25leoparddbContext _context;

        public LeopardProfileRepository(Su25leoparddbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<LeopardProfile> CreateAsync(LeopardProfile handbag)
        {
            _context.LeopardProfiles.Add(handbag);
            await _context.SaveChangesAsync();
            return handbag;
        }

        public async Task<LeopardProfile> UpdateAsync(LeopardProfile handbag)
        {
            _context.LeopardProfiles.Update(handbag);
            await _context.SaveChangesAsync();
            return handbag;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var handbag = await _context.LeopardProfiles.FindAsync(id);
            if (handbag == null) return false;

            _context.LeopardProfiles.Remove(handbag);
            await _context.SaveChangesAsync();
            return true;
        }
        

        public async Task<IQueryable<LeopardProfile>> GetQueryableAsync()
        {
            return _context.LeopardProfiles.Include(h => h.LeopardType).AsQueryable();
        }
    }
}

