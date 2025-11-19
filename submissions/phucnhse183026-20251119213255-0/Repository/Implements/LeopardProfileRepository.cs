using DAO.DTO;
using DAO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardProfileRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(LeopardProfile a)
        {
            await _context.LeopardProfiles.AddAsync(a);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await _context.LeopardProfiles.FindAsync(id);
            if (a != null)
            {
                _context.LeopardProfiles.Remove(a);
            }
            await SaveAsync();
        }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _context.LeopardProfiles.Include(n => n.LeopardType).ToListAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _context.LeopardProfiles.FindAsync(id);
        }

        public async Task UpdateAsync(LeopardProfile a)
        {
            _context.LeopardProfiles.Update(a);

            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
