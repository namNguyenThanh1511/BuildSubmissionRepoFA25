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
        private readonly LeopardProfileRepository _repository;
        public LeopardProfileService()
        {
            _repository = new LeopardProfileRepository();
        }

        public async Task<LeopardProfile?> GetById(int id)
        {
            var data = await _repository.GetAllWithIncludeAsync(x => x.LeopardType);

            return data.FirstOrDefault(x => x.LeopardProfileId == id);
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            var data = await _repository.GetAllWithIncludeAsync(x => x.LeopardType);

            return data.ToList();
        }

        public async Task<int> Create(LeopardProfile profile)
        {
            return await _repository.CreateAsync(profile);
        }

        public async Task<int> Update(LeopardProfile profile)
        {
            return await _repository.UpdateAsync(profile);
        }

        public async Task<bool> Delete(int id)
        {
            var entry = await _repository.GetByIdAsync(id);

            return await _repository.RemoveAsync(entry);
        }
    }
}
