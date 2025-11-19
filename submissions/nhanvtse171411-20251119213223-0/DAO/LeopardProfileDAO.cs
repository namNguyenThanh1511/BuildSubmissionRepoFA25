using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DAO
{
    public class LeopardProfileDAO
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileDAO(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeopardProfile>> GetAllProfilesAsync()
        {
            return await _context.LeopardProfiles.ToListAsync();
        }

        public async Task<LeopardProfile> GetProfileByIdAsync(int id)
        {
            return await _context.LeopardProfiles.FindAsync(id);
        }

        public async Task CreateProfileAsync(LeopardProfile profile)
        {
            _context.LeopardProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(LeopardProfile profile)
        {
            _context.LeopardProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(int id)
        {
            var profile = await _context.LeopardProfiles.FindAsync(id);
            if (profile != null)
            {
                _context.LeopardProfiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<LeopardProfile>> SearchProfilesAsync(string leopardName, string cheetahName, double? weight)
        {
            var query = _context.LeopardProfiles.AsQueryable();
            if (!string.IsNullOrEmpty(leopardName)) query = query.Where(p => p.LeopardName.Contains(leopardName));
            if (!string.IsNullOrEmpty(cheetahName)) query = query.Where(p => p.Characteristics.Contains(cheetahName));
            if (weight.HasValue) query = query.Where(p => p.Weight == weight.Value);
            return await query.ToListAsync();
        }
    }
}
