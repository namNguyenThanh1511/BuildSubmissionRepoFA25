using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardProfileService
    {
        private readonly LeopardProfileRepo _repository;

        public LeopardProfileService() => _repository = new LeopardProfileRepo();

        public async Task<int> Create(LeopardProfile item)
        {
            return await _repository.CreateAsync(item);
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

        public async Task<List<LeopardProfile>> Search(string? leopardName)
        {
            return await _repository.Search(leopardName);
        }

        public async Task<int> Update(LeopardProfile item)
        {
            return await (_repository.UpdateAsync(item));
        }
    }
}

