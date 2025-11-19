using BLL.DTOs;
using BLL.Responses;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardProfileService
    {
        private readonly LeopardProfileRepository _repository;

        public LeopardProfileService(LeopardProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllProfiles()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LeopardProfile> GetProfileByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<LeopardProfile> CreateProfileAsync(PostProfileDTO dto)
        {
            LeopardProfile profile = new LeopardProfile();
            profile.LeopardProfileId = dto.LeopardProfileId;
            profile.LeopardTypeId = dto.LeopardTypeId;
            profile.LeopardName = dto.LeopardName;
            profile.Weight = dto.Weight;
            profile.Characteristics = dto.Characteristics;
            profile.CareNeeds = dto.CareNeeds;
            profile.ModifiedDate = dto.ModifiedDate;
            return await _repository.CreateAsync(profile);
        }

        public async Task<LeopardProfile> UpdateProfileAsync(int id, LeopardProfile profile)
        {
            var profile1 = await _repository.GetByIdAsync(id);
            if (profile1 == null)
                throw new ApiException("HB40401", "Handbag not found");

            profile1.LeopardProfileId = profile.LeopardProfileId;
            profile1.LeopardTypeId = profile.LeopardTypeId;
            profile1.LeopardName= profile.LeopardName;
            profile1.Weight = profile.Weight;
            profile1.Characteristics = profile.Characteristics;
            profile1.CareNeeds = profile.CareNeeds;
            profile1.ModifiedDate = profile.ModifiedDate;

            return await _repository.UpdateAsync(profile1);
        }

        public async Task<bool> DeleteProfileAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
