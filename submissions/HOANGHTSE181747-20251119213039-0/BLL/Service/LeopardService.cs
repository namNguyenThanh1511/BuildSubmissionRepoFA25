using DAL;
using DAL.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class LeopardService : ILeopardService
    {
        private readonly ILeopardRepo _repo;
        public LeopardService(ILeopardRepo LeopardProfileRepo)
        {
            _repo = LeopardProfileRepo;
        }

        public async Task AddLeopardProfile(LeopardProfile LeopardProfile)
        {
            await _repo.AddLeopardProfile(LeopardProfile);
        }

        public async Task DeleteLeopardProfile(string id)
        {
            await _repo.DeleteLeopardProfile(id);
        }


        public async Task<LeopardProfile> GetLeopardProfile(string id)
        {
            return await _repo.GetLeopardProfile(id);
        }

        public async Task<List<LeopardProfile>> GetLeopardProfiles()
        {
            return await _repo.GetLeopardProfiles();
        }

        public async Task UpdateLeopardProfile(LeopardProfile LeopardProfile)
        {
            await _repo.UpdateLeopardProfile(LeopardProfile);
        }


    }
}
