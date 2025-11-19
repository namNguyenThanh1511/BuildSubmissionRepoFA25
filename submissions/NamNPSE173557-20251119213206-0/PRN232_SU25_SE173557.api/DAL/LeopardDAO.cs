using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LeopardDAO
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardDAO(SU25LeopardDBContext context)
        {
            _context = context;
        }


        // CREATE - Add new handbag
        public async Task<LeopardProfile> AddAsync(LeopardProfile leopardProfile)
        {
            var entity = await _context.LeopardTypes.FindAsync(leopardProfile.LeopardTypeId);

            if (entity == null)
            {
                throw new ArgumentException("Leopard does not exist.");
            }

            if (leopardProfile.Weight > 15)
            {
                throw new ArgumentException("Leopard weight must be more than 15.");
            }

            _context.LeopardProfiles.Add(leopardProfile);
            await _context.SaveChangesAsync();
            return leopardProfile;
        }

        // READ - Get all handbags
        public async Task<List<LeopardProfile>> GetAllHsAsync()
        {
            return await _context.LeopardProfiles
                                 .Include(h => h.LeopardType)
                                 .ToListAsync();
        }

        // READ - Get handbag by ID
        public async Task<LeopardProfile> GetByIdAsync(int leopardProfileId)
        {
            return await _context.LeopardProfiles
                                 .Include(h => h.LeopardType)
                                 .FirstOrDefaultAsync(h => h.LeopardProfileId == leopardProfileId);
        }

        // READ - Search handbags
        public async Task<List<LeopardProfile>> SearchAsync(string? leopardName)
        {
            var query = _context.LeopardProfiles
                                .Include(h => h.LeopardType)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(leopardName))
            {
                query = query.Where(h => h.LeopardName.Contains(leopardName));
            }
            var grouped = await query.ToListAsync();

            var result = grouped
                .GroupBy(h => h.LeopardType.LeopardTypeName)
                .Select(g => g.First())
                .ToList();

            return result;
        }

        // UPDATE - Update handbag
        public async Task<LeopardProfile> UpdateAsync(LeopardProfile leopardProfile)
        {
            var existing = await _context.LeopardProfiles.FindAsync(leopardProfile.LeopardProfileId);
            if (existing == null)
            {
                throw new ArgumentException("LeopardProfiles does not exist.");
            }

            var entity = await _context.LeopardTypes.FindAsync(leopardProfile.LeopardProfileId);
            if (entity == null)
            {
                throw new ArgumentException("LeopardType does not exist.");
            }
            if (leopardProfile.Weight > 15)
            {
                throw new ArgumentException("Leopard weight must be more than 15.");
            }

            existing.LeopardProfileId = leopardProfile.LeopardProfileId;
            existing.LeopardName = leopardProfile.LeopardName;
            existing.Weight = leopardProfile.Weight;
            existing.Characteristics = leopardProfile.Characteristics;
            existing.CareNeeds = leopardProfile.CareNeeds;
            existing.ModifiedDate = leopardProfile.ModifiedDate;


            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
            return existing;
        }

        // DELETE - Delete handbag
        public async Task<LeopardProfile> DeleteAsync(int Id)
        {
            var item = await _context.LeopardProfiles.FindAsync(Id);
            if (item != null)
            {
                _context.LeopardProfiles.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}