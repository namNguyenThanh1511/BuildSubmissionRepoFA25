using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173282.DAL.Model;

namespace PRN231_SU25_SE173282.DAL
{
    public class TypeRepository : GenericRepository<LeopardType>
    {
        private readonly Su25leopardDbContext _context;

        public TypeRepository() => _context ??= new Su25leopardDbContext();

        public async Task<LeopardType> GetBrandById(int id)
        {
            return await _context.LeopardTypes.SingleOrDefaultAsync(b => b.LeopardTypeId == id);
        }

    }
}
