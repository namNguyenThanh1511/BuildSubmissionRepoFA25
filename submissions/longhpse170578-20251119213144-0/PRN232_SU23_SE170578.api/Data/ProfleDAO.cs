using Microsoft.EntityFrameworkCore;
using PRN232_SU23_SE170578.api.Models;

namespace PRN232_SU23_SE170578.api.Data
{
    public class ProfleDAO
    {
        private static ProfleDAO? instance = null;
        private ProfleDAO() { }

        public static ProfleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfleDAO();
                }
                return instance;
            }
        }
        public async Task<List<LeopardProfile>> GetAllProfile()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var listProfile = await context.LeopardProfiles.Include(x => x.LeopardTypeId).ToListAsync();

                    return listProfile;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LeopardProfile> GetById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var resultObject = await context.LeopardProfiles
                    .FirstOrDefaultAsync(x => x.LeopardProfileId.Equals(id));
                return resultObject;
            }
        }
        public async Task<LeopardProfile> Update(LeopardProfile leopardProfile)
        {
            using (var context = new ApplicationDbContext())
            {
                var updateObject = await context.LeopardProfiles.FirstOrDefaultAsync(x => x.LeopardProfileId == leopardProfile.LeopardProfileId);
                if (updateObject == null)
                {
                    throw new Exception("CosmeticInformations not found");
                }

                //var cate = await context.LeopardProfiles.FirstOrDefaultAsync(x => x.CategoryId.Equals(leopardProfile.CategoryId));
                //if (cate == null)
                //{
                //    throw new Exception("Cate not found");
                //}

                updateObject.LeopardName = leopardProfile.LeopardName;
                updateObject.LeopardTypeId = leopardProfile.LeopardTypeId;
                updateObject.Weight = leopardProfile.Weight;
                updateObject.Characteristics = leopardProfile.Characteristics;
                updateObject.CareNeeds = leopardProfile.CareNeeds;
                //updateObject.ModifiedDate = ;

                context.LeopardProfiles.Update(updateObject);

                await context.SaveChangesAsync();
                return updateObject;
            }
        }
        public async Task<LeopardProfile> Delete(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var deleteObject = await context.LeopardProfiles.FirstOrDefaultAsync(p => p.LeopardProfileId.Equals(id));
                if (deleteObject == null)
                {
                    throw new Exception("Leopard Profile not found");
                }

                context.LeopardProfiles.Remove(deleteObject);

                await context.SaveChangesAsync();
                return deleteObject;
            }
        }
        public async Task<LeopardProfile> AddProfile(LeopardProfile profile)
        {
            using (var context = new ApplicationDbContext())
            {
                //var categoryObject = await context.LeopardProfiles.FirstOrDefaultAsync(x => x.CategoryId.Equals(cosmeticInformation.CategoryId));
                //if (categoryObject == null)
                //{
                //    throw new Exception("Category is not found");
                //}

                profile.LeopardProfileId = GenerateId();
                await context.LeopardProfiles.AddAsync(profile);
                await context.SaveChangesAsync();
                return profile;
            }
        }

        private int GenerateId()
        {
            using (var context = new ApplicationDbContext())
            {
                var countprof =  context.LeopardProfiles.Include(x => x.LeopardTypeId).Count();

                return countprof +1;
            }
        }
    }
}
