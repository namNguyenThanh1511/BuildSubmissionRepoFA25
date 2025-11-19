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
            return await _context.LeopardProfiles
                                 .Include(l => l.LeopardType)
                                 .ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int leopardProfileId)
        {
            return await _context.LeopardProfiles
                                 .Include(h => h.LeopardType)
                                 .FirstOrDefaultAsync(h => h.LeopardProfileId == leopardProfileId);
        }

        public async Task<LeopardProfile> AddAsync(LeopardProfile leopardProfile)
        {
            var existing = await _context.LeopardProfiles.FindAsync(leopardProfile.LeopardTypeId);

            if (existing == null)
            {
                throw new ArgumentException("Brand does not exist.");
            }

            if (leopardProfile.Weight <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }


            _context.LeopardProfiles.Add(leopardProfile);
            await _context.SaveChangesAsync();
            return leopardProfile;
        }

        public async Task<LeopardProfile> UpdateAsync(int id, LeopardProfile leopardProfile)
        {
            var existing = await _context.LeopardProfiles.FindAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("LeopardProfile does not exist.");
            }

            var existingtype = await _context.LeopardTypes.FindAsync(leopardProfile.LeopardTypeId);
            if (existingtype == null)
            {
                throw new ArgumentException("Brand does not exist.");
            }
            if (leopardProfile.Weight <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            existing.LeopardTypeId = leopardProfile.LeopardTypeId;
            existing.LeopardName = leopardProfile.LeopardName;
            existing.Characteristics = leopardProfile.Characteristics;
            existing.CareNeeds = leopardProfile.CareNeeds;
            existing.Weight = leopardProfile.Weight;
            existing.ModifiedDate = leopardProfile.ModifiedDate;


            _context.LeopardProfiles.Update(leopardProfile);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<LeopardProfile> DeleteAsync(int Id)
        {
            var leopardProfile = await _context.LeopardProfiles.FindAsync(Id);
            if (leopardProfile != null)
            {
                _context.LeopardProfiles.Remove(leopardProfile);
                await _context.SaveChangesAsync();
                return leopardProfile; 
            }
            return null; 
        }

        public async Task<List<LeopardProfile>> SearchAsync(string? modelName, double? weight)
        {
            var query = _context.LeopardProfiles
                                .Include(h => h.LeopardType) // GỌI TRƯỚC khi group
                                .AsQueryable();

            if (!string.IsNullOrEmpty(modelName))
            {
                query = query.Where(h => h.LeopardName.Contains(modelName));
            }

            if (weight <= 0)
            {
                query = query.Where(h => h.Weight == weight);
            }

            var grouped = await query
                .ToListAsync(); // đưa về memory

            var result = grouped
                .GroupBy(h => h.LeopardType.LeopardTypeName)
                .Select(g => g.First())
                .ToList();

            return result;
        }



/*        private async Task<int> GenerateUniqueIdAsync()
        {
            var random = new Random();
            int id;

            do
            {
                id = random.Next(10, 999999);
            }
            while (await _context.LeopardProfiles.AnyAsync(h => h.LeopardProfileId == id));

            return id;
        }*/

    }

}

