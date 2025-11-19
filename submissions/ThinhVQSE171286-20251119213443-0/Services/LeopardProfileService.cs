using Repositories;
using Repositories.DTOs;
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
        IQueryable<LeopardProfile> GetAllQueryable(LeopardProfileFilter? filter);
        Task<LeopardProfile> GetById(int id);
        Task<int> Create(LeopardProfile LeopardProfile);
        Task<int> Update(LeopardProfile LeopardProfile);
        Task<bool> Delete(int id);
    }
    public class LeopardProfileService : ILeopardProfileService
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

        public IQueryable<LeopardProfile> GetAllQueryable(LeopardProfileFilter? filter)
        {
            return _repository.GetAllQueryable(filter);
        }

        public async Task<int> Update(LeopardProfile LeopardProfile)
        {
            return await (_repository.UpdateAsync(LeopardProfile));
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}