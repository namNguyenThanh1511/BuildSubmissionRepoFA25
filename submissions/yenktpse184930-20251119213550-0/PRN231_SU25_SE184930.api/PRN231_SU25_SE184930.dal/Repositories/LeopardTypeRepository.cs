using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184930.dal.DBContext;
using PRN231_SU25_SE184930.dal.Interfaces;
using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.dal.Repositories
{
    public class LeopardTypeRepository : ILeopardTypeRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardTypeRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public async Task<LeopardType> GetByIdAsync(int id)
        {
            return await _context.LeopardTypes.FirstOrDefaultAsync(b => b.LeopardTypeId == id);
        }

        public async Task<IEnumerable<LeopardType>> GetAllAsync()
        {
            return await _context.LeopardTypes.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LeopardTypes.AnyAsync(b => b.LeopardTypeId == id);
        }
    }
}
