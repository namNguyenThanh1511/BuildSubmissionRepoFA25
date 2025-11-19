using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173362.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.DAL
{
    public class LeopardTypeRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardTypeRepository() => _context ??= new Su25leopardDbContext();

        public async Task<LeopardType> GetTypeById(int id)
        {
            return await _context.LeopardTypes.SingleOrDefaultAsync(b => b.LeopardTypeId == id);
        }

    }
}
