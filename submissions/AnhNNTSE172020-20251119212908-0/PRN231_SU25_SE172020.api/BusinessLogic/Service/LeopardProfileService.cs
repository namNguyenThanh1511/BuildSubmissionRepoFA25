using DataAccess.Models;
using DataAccess.Models.DTOs;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class LeopardProfileService : ILeopardProfileService
    {

        private readonly ILeopardProfileRepository _leopardProfileRepository;

        public LeopardProfileService(ILeopardProfileRepository leopardprofileRepository)
        {
            _leopardProfileRepository = leopardprofileRepository;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllLeopardsAsync()
        {
            return await _leopardProfileRepository.GetAllWithTypesAsync();
        }

        public async Task<LeopardProfile?> GetLeopardByIdAsync(int id)
        {
            return await _leopardProfileRepository.GetByIdWithTypesAsync(id);
        }

        public async Task<bool> CreateLeopardProfileAsync(LeopardProfileRequest leopardProfile)
        {
            LeopardProfile newLeopard = new LeopardProfile
            {
                LeopardProfileId = leopardProfile.LeopardProfileId,
                LeopardTypeId = leopardProfile.LeopardTypeId,
                LeopardName = leopardProfile.LeopardName,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate,
            };
            await _leopardProfileRepository.AddAsync(newLeopard);
            return await _leopardProfileRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateLeopardProfileAsync(int id, LeopardProfileRequest leopard)
        {
            var existingLeopard = await _leopardProfileRepository.GetByIdAsync(id);
            if (existingLeopard == null)
            {
                return false;
            }
            if (leopard.LeopardProfileId != null)
            {
                existingLeopard.LeopardProfileId = leopard.LeopardProfileId;
            }
            if (leopard.LeopardTypeId != null)
            {
                existingLeopard.LeopardTypeId = leopard.LeopardTypeId;
            }
            if (leopard.LeopardName != null)
            {
                existingLeopard.LeopardName = leopard.LeopardName;
            }
            if (leopard.Weight != null)
            {
                existingLeopard.Weight = leopard.Weight;
            }
            if (leopard.Characteristics != null)
            {
                existingLeopard.Characteristics = leopard.Characteristics;
            }
            if (leopard.CareNeeds != null)
            {
                existingLeopard.CareNeeds = leopard.CareNeeds;
            }
            if (leopard.ModifiedDate != null)
            {
                existingLeopard.ModifiedDate = leopard.ModifiedDate;
            }
            _leopardProfileRepository.Update(existingLeopard);
            return await _leopardProfileRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteLeopardProfileAsync(int id)
        {
            var leopard = await _leopardProfileRepository.GetByIdAsync(id);
            if (leopard == null)
            {
                return false;
            }

            _leopardProfileRepository.Delete(leopard);
            return await _leopardProfileRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Object>> SearchLeopardProfileAsync(string? LeopardName, double? Weight)
        {
            return await _leopardProfileRepository.SearchLeopardProfileAsync(LeopardName, Weight);
        }
    }
}
