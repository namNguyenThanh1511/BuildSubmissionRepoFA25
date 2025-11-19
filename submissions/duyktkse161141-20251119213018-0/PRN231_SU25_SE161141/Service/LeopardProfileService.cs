using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LeopardProfileService
    {
        private readonly SU25LeopardDBContext _dbContext;

        public LeopardProfileService(SU25LeopardDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LeopardProfileService()
        {
        }

        public async Task<List<LeopardProfile>> ListAllLeopardProfile(){

        return await _dbContext.LeopardProfiles.ToListAsync()??new List<LeopardProfile>();
        }

        public async Task<LeopardProfile> GetLeopardProfile(int id)
        => await _dbContext.LeopardProfiles.Where(lp => lp.LeopardProfileId == id).FirstOrDefaultAsync();

        public async Task<bool> CreateProfile(LeopardProfile profile)
        {
            var exist = _dbContext.LeopardProfiles.Where(lp => lp.LeopardProfileId == profile.LeopardProfileId).ToList();
            if (exist.Any()) return false;

            await _dbContext.LeopardProfiles.AddAsync(profile);
             _dbContext.SaveChanges();

            return true;
        }

        public async Task<int> UpdateProfile(LeopardProfile profile)
        {
            var exist = await _dbContext.LeopardProfiles.Where(lp => lp.LeopardProfileId == profile.LeopardProfileId).FirstOrDefaultAsync();
            if(profile == null) return 404;

            _dbContext.LeopardProfiles.Update(profile);
            _dbContext.SaveChanges();

            return 200;
        }

        public async Task<bool> DeleteProfile(int id)
        {
            var exist = await _dbContext.LeopardProfiles.Where(lp => lp.LeopardProfileId == id).FirstOrDefaultAsync();
            if (exist == null) return false;

            _dbContext.Remove(exist);
            _dbContext.SaveChanges();

            return true;
        }


    }
}
