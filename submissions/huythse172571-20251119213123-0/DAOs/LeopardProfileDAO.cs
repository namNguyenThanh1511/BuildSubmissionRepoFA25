using BOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class LeopardProfileDAO
    {
        private static LeopardProfileDAO instance = null;

        private LeopardProfileDAO()
        {

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

        public async Task<List<LeopardProfile>> GetLeopardProfiles()
        {
            using (var context = new Su25leopardDbContext())
            {
                var leopardprofiles = await context.LeopardProfiles.Include(p => p.LeopardType).ToListAsync();
                return leopardprofiles;
            }
        }

        public async Task<LeopardProfile> GetLeopardProfileById(int id)
        {
            using (var context = new Su25leopardDbContext())
            {
                var leopardprofile = await context.LeopardProfiles.Include(p => p.LeopardType).FirstOrDefaultAsync(leopardprofile => leopardprofile.LeopardProfileId == id);
                return leopardprofile;
            }
        }

        

        public async Task<LeopardProfile> AddLeopardProfile(LeopardProfile leopardProfile)
        {
            try
            {
                using (var context = new Su25leopardDbContext())
                {
                  
                    await context.LeopardProfiles.AddAsync(leopardProfile);
                    await context.SaveChangesAsync();
                    return leopardProfile;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<LeopardProfile> UpdateLeopardProfile(LeopardProfile leopardProfile)
        {
            using (var context = new Su25leopardDbContext())
            {
                var leopardProfileToUpdate = await context.LeopardProfiles.FirstOrDefaultAsync(p => p.LeopardProfileId == leopardProfile.LeopardProfileId);
                if (leopardProfileToUpdate == null)
                {
                    throw new Exception("LeopardProfile not found");
                }
                leopardProfileToUpdate.LeopardTypeId = leopardProfile.LeopardTypeId;
                leopardProfileToUpdate.LeopardName = leopardProfile.LeopardName;
                leopardProfileToUpdate.Weight = leopardProfile.Weight;
                leopardProfileToUpdate.Characteristics = leopardProfile.Characteristics;
                leopardProfileToUpdate.CareNeeds = leopardProfile.CareNeeds;
                leopardProfileToUpdate.ModifiedDate = leopardProfile.ModifiedDate;              



        context.Update(leopardProfileToUpdate);

                await context.SaveChangesAsync();
                return leopardProfileToUpdate;
            }
        }

        public async Task<LeopardProfile> DeleteLeopardProfile(int id)
        {
            try
            {
                using (var context = new Su25leopardDbContext())
                {
                    var leopardProfileToDelete = await context.LeopardProfiles.FirstOrDefaultAsync(p => p.LeopardProfileId.Equals(id));
                    if (leopardProfileToDelete == null)
                    {
                        throw new Exception("LeopardProfile not found");
                    }

                    context.LeopardProfiles.Remove(leopardProfileToDelete);
                    await context.SaveChangesAsync();

                    return leopardProfileToDelete;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
    }
