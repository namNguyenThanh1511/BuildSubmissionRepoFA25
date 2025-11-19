using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfileRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LeopardProfile> GetAll()
            => _context.LeopardProfiles
                       .ToList();

        public LeopardProfile GetById(int profileId)
            => _context.LeopardProfiles
                       .FirstOrDefault(h => h.LeopardProfileId == profileId);

        public LeopardProfile Create(LeopardProfile profile)
        {
            _context.LeopardProfiles.Add(profile);
            _context.SaveChanges();
            return profile;
        }

        public LeopardProfile Update(LeopardProfile profile)
        {
            _context.LeopardProfiles.Update(profile);
            _context.SaveChanges();
            return profile;
        }

        public void Delete(int profileId)
        {
            var e = _context.LeopardProfiles.Find(profileId);
            if (e == null) return;
            _context.LeopardProfiles.Remove(e);
            _context.SaveChanges();
        }

        public IQueryable<LeopardProfile> Search(string leopardName)
        {
            var q = _context.LeopardProfiles
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(leopardName))
                q = q.Where(leopard => EF.Functions.Like(leopard.LeopardName, $"%{leopardName}%"));

            return q;
        }
    }
}
