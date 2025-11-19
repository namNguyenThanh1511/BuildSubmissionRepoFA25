using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IMainService
    {
        Task<List<LeopardProfile>> GetAll();

        IQueryable<LeopardProfile> GetAllQueryable();

        Task<LeopardProfile> GetById(int id);
        Task<int> Create(LeopardProfile LeopardProfile);
        Task<int> Update(LeopardProfile LeopardProfile);
        Task<bool> Delete(int id);
        Task<int> GetNextLeopardProfileId();
    }

    public class LeopardProfileService : IMainService
    {
        private readonly LeopardProfileRepo _repository;

        public LeopardProfileService() => _repository = new LeopardProfileRepo();

        public async Task<int> Create(LeopardProfile LeopardProfile)
        {
            return await _repository.CreateAsync(LeopardProfile);
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return false;
            }

            return await _repository.RemoveAsync(item);
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public IQueryable<LeopardProfile> GetAllQueryable()
        {
            return _repository.GetAllQueryable();
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> GetNextLeopardProfileId()
        {
            return await _repository.GetNextLeopardProfileId();
        }

        public async Task<int> Update(LeopardProfile LeopardProfile)
        {
            return await (_repository.UpdateAsync(LeopardProfile));
        }
    }
}
