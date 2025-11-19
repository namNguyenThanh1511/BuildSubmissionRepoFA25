using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardProfileRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeopardProfile leopardProfile)
        {
            await _context.LeopardProfiles.AddAsync(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Remove(leopardProfile);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType).FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }

        public async Task<List<LeopardProfile>> SearchAsync(string modelName)
        {
            return await _context.LeopardProfiles.Include(h => h.LeopardType)
                  .Where(h =>
                      (string.IsNullOrEmpty(modelName) || h.LeopardName.Contains(modelName))).ToListAsync();

            
        }

        public async Task UpdateAsync(LeopardProfile leopardProfile)
        {
            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
        }

      
    }
}
