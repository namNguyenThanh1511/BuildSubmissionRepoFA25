using Repositories;
using Repositories.Dto;
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
        private readonly LeopardProfileRepository _repo;
        public LeopardProfileService() => _repo = new LeopardProfileRepository();

        public async Task<int> Create(LeopardProfile item)
        {
            return await _repo.CreateAsync(item);
        }

        public async Task<bool> Delete(int id)
        {
            var found = await _repo.GetByIdAsync(id);
            return await _repo.RemoveAsync(found);
        }

        public async Task<List<LeopardProfile>?> GetAll()
        {
            return (await _repo.GetAll()).ToList();
        }

        public async Task<LeopardProfile?> GetById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<int> Update(UpdateLeopardProfileDto item)
        {
            var profile = new LeopardProfile()
            {
                LeopardProfileId = item.LeopardProfileId,
                LeopardTypeId = item.LeopardTypeId,
                LeopardName = item.LeopardName,
                Characteristics = item.Characteristics,
                Weight = item.Weight,
                CareNeeds = item.CareNeeds,
                ModifiedDate = item.ModifiedDate,
            };
            return await _repo.UpdateAsync(profile);
        }

    }
}
