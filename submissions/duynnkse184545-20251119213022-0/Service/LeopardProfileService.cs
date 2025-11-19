using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTO;

namespace Service
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAll();
        Task<List<LeopardProfile>> GetAllWithType();
        Task<LeopardProfile> GetById(int id);
        Task<LeopardProfile> CreateProfile(LeopardProfileRequest leopardProfile);
        Task<LeopardProfile> UpdateProfile(int id, LeopardProfileRequest leopardProfile);
        Task DeleteProfile(int id);
        Task<List<LeopardType>> GetAllType();
        Task<object> SearchProfiles(string? leopardName, double? weight);
    }

    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly IGenericRepository<LeopardProfile, int> _profileRepo;
        private readonly IGenericRepository<LeopardType, int> _typeRepo;

        public LeopardProfileService(IGenericRepository<LeopardProfile, int> handbagRepo, IGenericRepository<LeopardType, int> brandRepo)
        {
            _profileRepo = handbagRepo;
            _typeRepo = brandRepo;
        }

        public async Task<LeopardProfile> CreateProfile(LeopardProfileRequest leopardProfile)
        {
            // Check if brand exists
            var brand = await _typeRepo.GetByIdAsync(leopardProfile.LeopardProfileId, b => b.LeopardTypeId == leopardProfile.LeopardProfileId);
            if (brand == null)
            {
                throw new ArgumentException("Invalid brandId - brand does not exist");
            }

            var profile = new LeopardProfile
            {
                LeopardProfileId = leopardProfile.LeopardProfileId,
                LeopardName = leopardProfile.LeopardName,
                LeopardTypeId = leopardProfile.LeopardTypeId,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate,
            };

            try
            {
                await _profileRepo.CreateAsync(profile);
                return profile;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to create handbag: {ex.Message}");
            }
        }

        public async Task DeleteProfile(int id)
        {
            // Validate ID
            if (id <= 0)
            {
                throw new ArgumentException("Invalid profile ID");
            }

            var handbag = await _profileRepo.GetByIdAsync(id, h => h.LeopardProfileId == id);
            if (handbag == null)
            {
                throw new KeyNotFoundException("Profile not found");
            }

            try
            {
                await _profileRepo.RemoveAsync(handbag);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to delete profile: {ex.Message}");
            }
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            try
            {
                return await _profileRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to retrieve profiles: {ex.Message}");
            }
        }

        public async Task<List<LeopardProfile>> GetAllWithType()
        {
            try
            {
                return await _profileRepo.GetAllAsync(query => query.Include(h => h.LeopardType));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to retrieve profile with types: {ex.Message}");
            }
        }

        public async Task<List<LeopardType>> GetAllType ()
        {
            try
            {
                return await _typeRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to retrieve types: {ex.Message}");
            }
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            // Validate ID
            if (id <= 0)
            {
                throw new ArgumentException("Invalid profile ID");
            }

            var handbag = await _profileRepo.GetByIdAsync(id, h => h.LeopardProfileId == id,
                query => query.Include(h => h.LeopardType));

            if (handbag == null)
            {
                throw new KeyNotFoundException("Profile not found");
            }

            return handbag;
        }

        public async Task<LeopardProfile> UpdateProfile(int id, LeopardProfileRequest leopardProfile)
        {
            // Validate ID
            if (id <= 0)
            {
                throw new ArgumentException("Invalid profile ID");
            }

            var existingProfile = await _profileRepo.GetByIdAsync(id, h => h.LeopardProfileId == id,
                query => query.Include(h => h.LeopardType));

            if (existingProfile == null)
            {
                throw new KeyNotFoundException("Profiles not found");
            }

            // Check if brand exists
            var brand = await _typeRepo.GetByIdAsync(leopardProfile.LeopardTypeId, b => b.LeopardTypeId == leopardProfile.LeopardTypeId);
            if (brand == null)
            {
                throw new ArgumentException("Invalid brandId - brand does not exist");
            }

            try
            {
                await _profileRepo.UpdateAsync(existingProfile);
                return existingProfile;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to update profile: {ex.Message}");
            }
        }

        public async Task<object> SearchProfiles(string? leopardName, double? weight)
        {
            try
            {
                var profiles = await _profileRepo.GetAllAsync(query => query.Include(h => h.LeopardType));

                // Filter by modelName and material if provided
                if (!string.IsNullOrEmpty(leopardName))
                {
                    profiles = profiles.Where(h => h.LeopardName.Contains(leopardName, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (weight != null)
                {
                    profiles = profiles.Where(h => h.Weight != null && h.Weight.Equals(weight)).ToList();
                }

                return profiles;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Failed to search profiless: {ex.Message}");
            }
        }
    }
}
