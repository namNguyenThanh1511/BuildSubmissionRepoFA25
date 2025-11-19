using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LeoPardRepository : ILeoPardRepository
    {
        private readonly SU25LeopardDBContext _context;

        public LeoPardRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public IEnumerable<LeopardProfile> GetAll()
        {
            return _context.LeopardProfiles.Include(h => h.LeopardType).ToList();
        }

        public LeopardProfile? GetById(int id)
        {
            return _context.LeopardProfiles.Include(h => h.LeopardType).FirstOrDefault(h => h.LeopardProfileId == id);
        }

        public void Add(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Add(LeopardProfile);
        }

        public void Update(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Update(LeopardProfile);
        }

        public void Delete(LeopardProfile LeopardProfile)
        {
            _context.LeopardProfiles.Remove(LeopardProfile);
        }

        public bool Exists(int id)
        {
            return _context.LeopardProfiles.Any(h => h.LeopardProfileId == id);
        }

        public IEnumerable<LeopardProfile> Search(string leopardName, double? weight)
        {
            return _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .Where(h =>
                    (string.IsNullOrEmpty(leopardName) || h.LeopardName.Contains(leopardName)) &&
                    (!weight.HasValue || h.Weight == weight.Value))
                .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
