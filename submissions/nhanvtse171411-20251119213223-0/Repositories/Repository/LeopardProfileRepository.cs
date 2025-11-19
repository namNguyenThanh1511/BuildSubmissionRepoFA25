using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAO;
using Models.Models;
using Repositories.Interfaces;

namespace Repositories.Repository
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        private readonly LeopardProfileDAO _dao;

        public LeopardProfileRepository()
        {
            Su25leopardDbContext context = new Su25leopardDbContext();
            _dao = new LeopardProfileDAO(context);
        }

        public async Task<List<LeopardProfile>> GetAllProfilesAsync()
        {
            return await _dao.GetAllProfilesAsync();
        }

        public async Task<LeopardProfile> GetProfileByIdAsync(int id)
        {
            return await _dao.GetProfileByIdAsync(id);
        }

        public async Task CreateProfileAsync(LeopardProfile profile)
        {
            await _dao.CreateProfileAsync(profile);
        }

        public async Task UpdateProfileAsync(LeopardProfile profile)
        {
            await _dao.UpdateProfileAsync(profile);
        }

        public async Task DeleteProfileAsync(int id)
        {
            await _dao.DeleteProfileAsync(id);
        }

        public async Task<List<LeopardProfile>> SearchProfilesAsync(string leopardName, string cheetahName, double? weight)
        {
            return await _dao.SearchProfilesAsync(leopardName, cheetahName, weight);
        }
    }
}
