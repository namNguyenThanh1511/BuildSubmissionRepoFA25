using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class LeopardTypeDAO
    {
        private Su25leopardDbContext _dbContext;
        private static LeopardTypeDAO instance;

        public LeopardTypeDAO()
        {
            _dbContext = new Su25leopardDbContext();
        }

        public static LeopardTypeDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardTypeDAO();
                }
                return instance;
            }
        }

        private Su25leopardDbContext CreateDbContext()
        {
            return new Su25leopardDbContext();
        }

        public List<LeopardType> GetLeopardTypes()
        {
            using (var dbContext = CreateDbContext())
            {
                return dbContext.LeopardTypes.AsNoTracking().Include(manufacturer => manufacturer.LeopardProfiles).ToList();
            }
        }

        public LeopardType GetLeopardTypeById(int? id)
        {
            using (var dbContext = CreateDbContext())
            {
                return dbContext.LeopardTypes.AsNoTracking()
                    .SingleOrDefault(m => m.LeopardTypeId.Equals(id));
            }
        }
    }
}
