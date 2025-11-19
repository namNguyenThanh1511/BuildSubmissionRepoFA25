using Repositories.DTOs;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _profileRepository;

        public LeopardProfileService(ILeopardProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<IEnumerable<LeopardProfileResponseDTO>> GetAllProfilesAsync()
        {
            var profiles = await _profileRepository.GetAllProfilesAsync();
            return profiles.Select(MapToResponseDTO);
        }

        public async Task<LeopardProfileResponseDTO?> GetProfileByIdAsync(int profileId)
        {
            var profile = await _profileRepository.GetProfileByIdAsync(profileId);
            return profile != null ? MapToResponseDTO(profile) : null;
        }

        public async Task<LeopardProfileResponseDTO> CreateProfileAsync(LeopardProfileDTO profileDto)
        {
            var profile = new LeopardProfile
            {
                LeopardTypeId = profileDto.LeopardTypeId,
                LeopardName = profileDto.LeopardName,
                Weight = profileDto.Weight,
                Characteristics = profileDto.Characteristics,
                CareNeeds = profileDto.CareNeeds,
                ModifiedDate = DateTime.Now
            };

            var createdProfile = await _profileRepository.CreateProfileAsync(profile);
            return MapToResponseDTO(createdProfile);
        }

        public async Task<LeopardProfileResponseDTO> UpdateProfileAsync(int id, LeopardProfileDTO profileDto)
        {
            var existingProfile = await _profileRepository.GetProfileByIdAsync(id);
            if (existingProfile == null)
                throw new ArgumentException("Profile not found");

            existingProfile.LeopardTypeId = profileDto.LeopardTypeId;
            existingProfile.LeopardName = profileDto.LeopardName;
            existingProfile.Weight = profileDto.Weight;
            existingProfile.Characteristics = profileDto.Characteristics;
            existingProfile.CareNeeds = profileDto.CareNeeds;
            existingProfile.ModifiedDate = DateTime.Now;

            var updatedProfile = await _profileRepository.UpdateProfileAsync(existingProfile);
            return MapToResponseDTO(updatedProfile);
        }

        public async Task<bool> DeleteProfileAsync(int profileId)
        {
            return await _profileRepository.DeleteProfileAsync(profileId);
        }

        public async Task<IEnumerable<LeopardProfileResponseDTO>> SearchProfilesAsync(string? leopardName, double? weight)
        {
            var profiles = await _profileRepository.SearchProfilesAsync(leopardName, weight);
            return profiles.Select(MapToResponseDTO);
        }

        private LeopardProfileResponseDTO MapToResponseDTO(LeopardProfile profile)
        {
            return new LeopardProfileResponseDTO
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardTypeId = profile.LeopardTypeId,
                LeopardName = profile.LeopardName,
                Weight = profile.Weight,
                Characteristics = profile.Characteristics,
                CareNeeds = profile.CareNeeds,
                ModifiedDate = profile.ModifiedDate,
                LeopardType = profile.LeopardType != null ? new LeopardTypeResponseDTO
                {
                    LeopardTypeId = profile.LeopardType.LeopardTypeId,
                    LeopardTypeName = profile.LeopardType.LeopardTypeName ?? string.Empty,
                    Origin = profile.LeopardType.Origin ?? string.Empty,
                    Description = profile.LeopardType.Description ?? string.Empty
                } : null
            };
        }
    }
}