using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class LeopardProfileDAO
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardProfileDAO(SU25LeopardDBContext context)
        {
            _context = context;
        }


        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfile
                                 .Include(h => h.LeopardType)
                                 .ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfile
                                 .Include(h => h.LeopardType)
                                 .FirstOrDefaultAsync(h => h.LeopardProfileId == id);
        }
  
        public async Task<LeopardProfile> AddAsync(LeopardProfile obj)
        {
            var existingObject = await _context.LeopardType.FindAsync(obj.LeopardTypeId);

            if (existingObject == null)
            {
                throw new ArgumentException("Type does not exist.");
            }

            _context.LeopardProfile.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<LeopardProfile> UpdateAsync(LeopardProfile obj)
        {
            var existingObject = await _context.LeopardProfile.FindAsync(obj.LeopardProfileId);
            if (existingObject == null)
            {
                throw new ArgumentException("LeopardProfile does not exist.");
            }

            var existingItem = await _context.LeopardType.FindAsync(obj.LeopardTypeId);
            if (existingItem == null)
            {
                throw new ArgumentException("Type does not exist.");
            }

            existingObject.LeopardName = obj.LeopardName;
            existingObject.Weight = obj.Weight;
            existingObject.Characteristics = obj.Characteristics;
            existingObject.CareNeeds = obj.CareNeeds;
            existingObject.ModifiedDate = obj.ModifiedDate;
            existingObject.LeopardTypeId = obj.LeopardTypeId;
              
        await _context.SaveChangesAsync();
            return existingObject;
        }
        public async Task<LeopardProfile> DeleteAsync(int id)
        {
            var obj = await _context.LeopardProfile.FindAsync(id);
            if (obj != null)
            {
                _context.LeopardProfile.Remove(obj);
                await _context.SaveChangesAsync();
                return obj;
            }
            return null;
        }

        public async Task<List<LeopardProfile>> SearchAsync(string? field1, string? field2)
        {
            var query = _context.LeopardProfile
                                .Include(h => h.LeopardType)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(field1))
            {
                query = query.Where(h => h.LeopardName.Contains(field1));
            }

            if (!string.IsNullOrEmpty(field2) && int.TryParse(field2, out int weight))
            {
                query = query.Where(h => h.Weight == weight);
            }

            var grouped = await query
                .ToListAsync();

            var result = grouped
                .GroupBy(h => h.LeopardType.LeopardTypeName)
                .Select(g => g.First())
                .ToList();

            return result;
        }

    }
  
}


