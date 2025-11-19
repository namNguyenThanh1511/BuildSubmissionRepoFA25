using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        Task<int> CreateAsync(LeopardProfile LeopardProfile);
        Task<int> UpdateAsync(LeopardProfile LeopardProfile);
        Task<bool> DeleteAsync(int id);
        Task<List<LeopardProfile>> SearchAsync(string? LeopardName, double? Weight);
        IQueryable<LeopardProfile> GetQueryable();

    }
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly LeopardProfileRepository _repository;

        public LeopardProfileService()
        {
            _repository = new LeopardProfileRepository();
        }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(LeopardProfile LeopardProfile)

        {
            
            return await _repository.CreateAsync(LeopardProfile);
        }

        public async Task<int> UpdateAsync(LeopardProfile LeopardProfile)
        {
            return await _repository.UpdateAsync(LeopardProfile);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return false;
            }
            return await _repository.RemoveAsync(item);
        }

        public async Task<List<LeopardProfile>> SearchAsync(string? LeopardName, double? Weight)
        {
            return await _repository.SearchAsync(LeopardName, Weight);
        }
        public IQueryable<LeopardProfile> GetQueryable()
        {
            return _repository.GetQueryable();
        }

    }

}
