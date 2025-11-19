using Microsoft.EntityFrameworkCore;
using Repositories.Basic;
using Repositories.DTO;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardProfileRepository : GenericRepository<LeopardProfile>
    {
        public LeopardProfileRepository() { }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            var items = await _context.LeopardProfiles.Include(c => c.LeopardType).ToListAsync();

            return items;
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            var item = await _context.LeopardProfiles.Include(c => c.LeopardType).FirstOrDefaultAsync(c => c.LeopardProfileId == id);

            return item;
        }

        public async Task<List<LeopardProfile>> SearchAsync(string name, double? weight)
        {
            var items = await _context.LeopardProfiles
                .Include(c => c.LeopardType)
                .Where(po =>
                (po.LeopardName.Contains(name) || string.IsNullOrEmpty(name))
                && (po.Weight == weight || weight == 0 || weight == null)
             ).ToListAsync();

            return items;
        }

        public async Task<int> Add(LeopardProfileDTO itemDTO)
        {
            try
            {
                _context.ChangeTracker.Clear();
                int newId = _context.LeopardProfiles.Max(h => (int?)h.LeopardProfileId) ?? 0;
                newId += 1;

                var leopard = new LeopardProfile
                {
                    LeopardName = itemDTO.LeopardName,
                    Weight = itemDTO.Weight,
                    Characteristics = itemDTO.Characteristics,
                    CareNeeds = itemDTO.CareNeeds,
                    ModifiedDate = DateTime.Now,
                    LeopardTypeId = itemDTO.LeopardTypeId,
                };

                await _context.LeopardProfiles.AddAsync(leopard);
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add Leopard: {e.Message}", e);
            }
        }

        public async Task<int> Update(LeopardProfileDTO itemDTO)
        {
            var existing = await _context.LeopardProfiles.FirstOrDefaultAsync(na => na.LeopardProfileId == itemDTO.LeopardProfileId);

            if (existing != null)
            {
                existing.LeopardName = itemDTO.LeopardName;
                existing.Weight = itemDTO.Weight;
                existing.Characteristics = itemDTO.Characteristics;
                existing.CareNeeds = itemDTO.CareNeeds;
                existing.ModifiedDate = itemDTO.ModifiedDate;
                existing.LeopardTypeId = itemDTO.LeopardTypeId;
                return await _context.SaveChangesAsync();
            }

            throw new Exception($"Failed to Update Leopard: No Leopard Found");
        }
    }
}
