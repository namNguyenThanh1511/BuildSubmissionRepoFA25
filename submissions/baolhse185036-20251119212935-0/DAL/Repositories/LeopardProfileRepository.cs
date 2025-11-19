using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LeopardProfile> GetAll()
        {
            return _context.LeopardProfiles.Include(lp => lp.LeopardType).ToList();
        }

        public LeopardProfile GetById(int id)
        {
            return _context.LeopardProfiles.Include(lp => lp.LeopardType).FirstOrDefault(lp => lp.LeopardProfileId == id);
        }

        public void Delete(int id)
        {
            var leopardProfile = _context.LeopardProfiles.Find(id);
            if (leopardProfile != null)
            {
                _context.LeopardProfiles.Remove(leopardProfile);
                _context.SaveChanges();
            }
        }
    }
}
