using Repositories.Interfaces;
using Repositories.Models;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly ILeoPardProfileRepository _repository;

        public ProfileService(ILeoPardProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
           return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProfileModel>> GetAllAsync()
        {
            var profile = await _repository.GetAllAsync();
            return profile.Select(MapToResponse);
        }

        public async Task<ProfileModel> GetByIdAsync(int id)
        {
            var profile = await _repository.GetByIdAsync(id);
            return profile != null ? MapToResponse(profile) : null;
        }

        private ProfileModel MapToResponse(LeopardProfile leo)
        {
            return new ProfileModel
            {
                LeopardProfileId = leo.LeopardProfileId,
                LeopardName = leo.LeopardName,
                LeopardTypeId = leo.LeopardTypeId,
                Weight = leo.Weight,
                Characteristics = leo.Characteristics,
                CareNeeds = leo.CareNeeds,
                ModifiedDate = leo.ModifiedDate,

                type = leo.LeopardType != null ? new TypeModel
                {
                    LeopardTypeId = leo.LeopardType.LeopardTypeId,
                    LeopardTypeName= leo.LeopardType.LeopardTypeName,
                    Origin = leo.LeopardType.Origin,
                    Description=leo.LeopardType.Description,
    } : null
            };
        }
    }
}
