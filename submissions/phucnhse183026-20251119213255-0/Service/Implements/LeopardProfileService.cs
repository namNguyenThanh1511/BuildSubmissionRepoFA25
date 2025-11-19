using DAO.DTO;
using DAO.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implements
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repository;

        public LeopardProfileService(ILeopardProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(CreateProfile dto)
        {
            var profile = new LeopardProfile
            {
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate
            };

            await _repository.CreateAsync(profile);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<LeopardProfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LeopardProfile> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(LeopardProfile a)
        {
            await _repository.UpdateAsync(a);
        }
    }
}
