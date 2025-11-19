using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class LeopardProfileDAO
    {
        private Su25leopardDbContext _dbContext;
        private static LeopardProfileDAO instance;

        public LeopardProfileDAO()
        {
            _dbContext = new Su25leopardDbContext();
        }

        public static LeopardProfileDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardProfileDAO();
                }
                return instance;
            }
        }

        private Su25leopardDbContext CreateDbContext()
        {
            return new Su25leopardDbContext();
        }

        public LeopardProfile GetLeopardProfileById(int? id)
        {
            using (var dbContext = CreateDbContext())
            {
                return dbContext.LeopardProfiles.AsNoTracking()
                    .Include(m => m.LeopardType)
                    .SingleOrDefault(m => m.LeopardProfileId.Equals(id));
            }
        }

        public LeopardProfile GetLeopardProfileByName(string? name)
        {
            using (var dbContext = CreateDbContext())
            {
                return dbContext.LeopardProfiles.AsNoTracking()
                    .Include(m => m.LeopardType)
                    .SingleOrDefault(m => m.LeopardName.Equals(name));
            }
        }

        public List<LeopardProfile> GetAllLeopardProfiles()
        {
            using (var dbContext = CreateDbContext())
            {
                return dbContext.LeopardProfiles.AsNoTracking()
                    .Include(m => m.LeopardType)
                    .ToList();
            }
        }

        public void AddLeopardProfile(LeopardProfile LeopardProfile)
        {
            using (var dbContext = CreateDbContext())
            {
                LeopardProfile currentLeopardProfile = dbContext.LeopardProfiles
                    .AsNoTracking()
                    .FirstOrDefault(h => h.LeopardName == LeopardProfile.LeopardName);

                if (currentLeopardProfile == null)
                {
                    dbContext.LeopardProfiles.Add(LeopardProfile);
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateLeopardProfile(int LeopardProfileId, LeopardProfile LeopardProfile)
        {
            using (var dbContext = CreateDbContext())
            {
                LeopardProfile currentLeopardProfile = GetLeopardProfileById(LeopardProfileId);
                if (currentLeopardProfile != null)
                {
                    dbContext.LeopardProfiles.Update(LeopardProfile);
                    dbContext.SaveChanges();
                }
            }
        }

        public void DeleteLeopardProfile(int LeopardProfileId)
        {
            using (var dbContext = CreateDbContext())
            {
                LeopardProfile currentLeopardProfile = GetLeopardProfileById(LeopardProfileId);
                if (currentLeopardProfile != null)
                {
                    dbContext.LeopardProfiles.Remove(currentLeopardProfile);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
