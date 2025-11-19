using BusinessObjects.Models;
using DTOs;
using Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HandbagService
    {
        private readonly ILeopardProfileRepo _repo;
        public HandbagService(ILeopardProfileRepo repo) => _repo = repo;

        public async Task<IEnumerable<LeopardProfileDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(h => new LeopardProfileDTO
            {
                LeopaProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate
            });
        }

        public async Task<LeopardProfileDTO> GetByIdAsync(int id)
        {
            var h = await _repo.GetByIdAsync(id);
            if (h == null) return null;
            return new LeopardProfileDTO
            {
                LeopaProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate
            };
        }

        public async Task CreateAsync(LeopardProfileDTO h)
        {
            var t = new LeopardProfile
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate
            };
            await _repo.AddAsync(t);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

    }
}
