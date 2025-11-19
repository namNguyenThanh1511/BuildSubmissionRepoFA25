using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class LeopardService : ILeopardService
    {
        private readonly IGenericRepository<LeopardProfile> _profileRepository;

        public LeopardService(IGenericRepository<LeopardProfile> profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<LeopardResponse> CreateAsync(LeopardRequest request)
        {
            request.Validate();

            var profile = new LeopardProfile
            {
                LeopardTypeId = request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                ModifiedDate = request.ModifiedDate
            };

            await _profileRepository.InsertAsync(profile);
            await _profileRepository.SaveAsync();

            return new LeopardResponse
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardTypeId = profile.LeopardTypeId,
                LeopardName = profile.LeopardName,
                Weight = profile.Weight,
                Characteristics = profile.Characteristics,
                CareNeeds = profile.CareNeeds,
                ModifiedDate = profile.ModifiedDate
            };
        }

        public async Task DeleteAsync(int id)
        {
            var existingProfile = await _profileRepository.Entities
                .FirstOrDefaultAsync(hb => hb.LeopardProfileId == id);

            if (existingProfile == null)
            {
                throw new KeyNotFoundException($"Profile with ID {id} not found.");
            }

            _profileRepository.Delete(existingProfile.LeopardProfileId);
            await _profileRepository.SaveAsync();
        }

        public async Task<IEnumerable<LeopardResponse>> GetAllAsync()
        {
            var query = _profileRepository.Entities.AsNoTracking();

            var profiles = await query.Select(pf => new LeopardResponse
            {
                LeopardProfileId = pf.LeopardProfileId,
                LeopardTypeId = pf.LeopardTypeId,
                LeopardName = pf.LeopardName,
                Weight = pf.Weight,
                Characteristics = pf.Characteristics,
                CareNeeds = pf.CareNeeds,
                ModifiedDate = pf.ModifiedDate
            }).ToListAsync();

            return profiles;
        }

        public async Task<LeopardResponse> GetByIdAsync(int id)
        {
            var profile = await _profileRepository.Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(pf => pf.LeopardProfileId == id);

            if (profile == null)
            {
                throw new KeyNotFoundException($"Profile with ID {id} not found.");
            }

            return new LeopardResponse
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardTypeId = profile.LeopardTypeId,
                LeopardName = profile.LeopardName,
                Weight = profile.Weight,
                Characteristics = profile.Characteristics,
                CareNeeds = profile.CareNeeds,
                ModifiedDate = profile.ModifiedDate
            };
        }

        public async Task<IEnumerable<LeopardResponse>> SearchAsync(string? leopardName, double? weight)
        {
            var query = _profileRepository.Entities.AsNoTracking();

            if (!string.IsNullOrEmpty(leopardName))
            {
                query = query.Where(p => p.LeopardName.ToLower().Contains(leopardName.ToLower()));
            }

            if (weight.HasValue)
            {
                query = query.Where(p => p.Weight == weight);
            }

            var result = await query.Select(p => new LeopardResponse
            {
                LeopardProfileId = p.LeopardProfileId,
                LeopardTypeId = p.LeopardTypeId,
                LeopardName = p.LeopardName,
                Weight = p.Weight,
                Characteristics = p.Characteristics,
                CareNeeds = p.CareNeeds,
                ModifiedDate = p.ModifiedDate
            }).ToListAsync();

            return result;
        }

        public async Task UpdateAsync(int id, LeoparUpdatedRequest request)
        {
            request.Validate();

            var existProfile = await _profileRepository.Entities
                .FirstOrDefaultAsync(pf => pf.LeopardProfileId == id);

            if (existProfile == null)
            {
                throw new KeyNotFoundException($"Profile with ID {id} not found.");
            }

            existProfile.LeopardTypeId = request.LeopardTypeId;
            existProfile.LeopardName = request.LeopardName;
            existProfile.Weight = request.Weight;
            existProfile.Characteristics = request.Characteristics;
            existProfile.CareNeeds = request.CareNeeds;
            existProfile.ModifiedDate = request.ModifiedDate;

            _profileRepository.Update(existProfile);
            await _profileRepository.SaveAsync();
        }
    }
}

