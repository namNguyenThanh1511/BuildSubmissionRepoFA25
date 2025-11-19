using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LeopardProfileRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardProfileRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public int GetLatestId()
        {
            return _context.LeopardProfile.Max(x => x.LeopardProfileId);
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            return await _context.LeopardProfile.Include(x => x.LeopardType).ToListAsync();
        }

        public async Task<IQueryable<LeopardProfile>> GetAllQueryable()
        {
            return _context.LeopardProfile.Include(x => x.LeopardType).AsQueryable();
        }

        public async Task<LeopardProfile?> GetById(int id)
        {
            return await _context.LeopardProfile.Include(x => x.LeopardType).Where(x => x.LeopardProfileId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Create(LeopardProfile obj)
        {
            await _context.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> Update(LeopardProfile obj)
        {
            _context.Update(obj);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> Delete(LeopardProfile obj)
        {
            _context.Remove(obj);
            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
