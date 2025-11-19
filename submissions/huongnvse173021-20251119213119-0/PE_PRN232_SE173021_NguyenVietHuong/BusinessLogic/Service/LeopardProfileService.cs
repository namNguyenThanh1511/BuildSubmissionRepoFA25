using DataAccess.DTOs;
using DataAccess.Entities;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class LeopardProfileService
    {
        private readonly LeopardProfileRepositories _repo;
        public LeopardProfileService(LeopardProfileRepositories repo)
        {
            _repo = repo;
        }
        public async Task<List<LeopardProfileDTO>> GetAllLeopardProfileAsync()
        {
            return await _repo.GetAllLeopardProfileAsync();
        }
        public async Task CreateLeopardProfileAsync(LeopardProfile LeopardProfile)
        {
            await _repo.CreateLeopardProfileAsync(LeopardProfile);
        }
        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<bool> UpdateAsync(LeopardProfile LeopardProfile)
        {
            var result = await _repo.UpdateAsync(LeopardProfile);
            return result;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
        public async Task<List<LeopardProfileDTO>> SearchAsync(string? modelName, double? weight)
        {
            return await _repo.SearchAsync(modelName, weight);
        }
    }
}
