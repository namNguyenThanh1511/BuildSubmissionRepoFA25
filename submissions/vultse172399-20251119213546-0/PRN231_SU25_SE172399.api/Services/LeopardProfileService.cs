using Repositories;
using Repositories.DTO;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly LeopardProfileRepository _repository;

        public LeopardProfileService() => _repository = new LeopardProfileRepository();

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var leopard = await _repository.GetByIdAsync(id);

            return await _repository.RemoveAsync(leopard);
        }

        public async Task<int> CreateAsync(LeopardProfileDTO leopard)
        {
            var result = await _repository.Add(leopard);

            return result;
        }

        public async Task<int> UpdateAsync(LeopardProfileDTO leopard)
        {
            var result = await _repository.Update(leopard);

            return result;
        }

        public async Task<List<LeopardProfile>> SearchAsync(string name, double? weight)
        {
            var items = await _repository.SearchAsync(name, weight);

            if (items != null)
            {
                return items;
            }

            return new List<LeopardProfile>();
        }
    }
}
