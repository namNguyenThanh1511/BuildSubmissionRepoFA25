using BLL.Interface;
using DAL.DTOs;
using DAL.Interface;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task AddHandbagAsync(ProfileDTOCreate profileDTO)
        {
            var profile = new LeopardProfile
            {
                LeopardProfileId = profileDTO.LeopardProfileId,
                LeopardName = profileDTO.LeopardName,
                CareNeeds = profileDTO.CareNeeds,
                Characteristics = profileDTO.Characteristics,
                LeopardTypeId = profileDTO.LeopardTypeId,
                ModifiedDate = profileDTO.ModifiedDate,
                Weight = profileDTO.Weight
            };
            await _profileRepository.AddProfileAsync(profile);
        }

        public async Task DeleteHandbagAsync(int id)
        {
            await _profileRepository.DeleteProfileAsync(id);
        }

        public async Task<IEnumerable<ProfileDTOs>> GetAllHandbagsAsync()
        {
            var handbags = await _profileRepository.GetAllProfilesAsync();
            var result = handbags.Select(h => new ProfileDTOs
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardName = h.LeopardName,
                CareNeeds = h.CareNeeds,
                Characteristics = h.Characteristics,
                LeopardTypeId = h.LeopardTypeId,
                Weight = h.Weight,
                ModifiedDate = h.ModifiedDate

            });
            return result;
        }

        public async Task<ProfileDTOs> GetHandbagByIdAsync(int id)
        {
            var profile = await _profileRepository.GetProfileByIdAsync(id);
            var result = new ProfileDTOs
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardName = profile.LeopardName,
                CareNeeds = profile.CareNeeds,
                Characteristics = profile.Characteristics,
                LeopardTypeId = profile.LeopardTypeId,
                Weight = profile.Weight,
                ModifiedDate = profile.ModifiedDate
            };
            return result;
        }

        public async Task<IEnumerable<ProfileDTOs>> SearchByNameAndWeight(string? name, double? weight)
        {
            var profile = await _profileRepository.SearchByNameAndWeight(name, weight);
            return profile;
        }

        public async Task UpdateProfileAsync(int id, ProfileDTOCreate profile)
        {
            var profile1 = await _profileRepository.GetProfileByIdAsync(id);
            profile1.LeopardProfileId = profile.LeopardProfileId;
            profile1.LeopardName = profile.LeopardName;

            profile1.CareNeeds = profile.CareNeeds;
            profile1.Characteristics = profile.Characteristics;
            profile1.LeopardTypeId = profile.LeopardTypeId;
            profile1.Weight = profile.Weight;
            profile1.ModifiedDate = profile.ModifiedDate;
            await _profileRepository.UpdateProfileAsync(profile1);
        }
    }
}
