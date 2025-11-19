using Microsoft.IdentityModel.Tokens;
using Repo.Base;
using Repo.Models;
using Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LeopardProfileService
    {
        private readonly GenericRepository<LeopardProfile> _repo;
        private readonly GenericRepository<LeopardType> _typeRepo;

        public LeopardProfileService(GenericRepository<LeopardProfile> repo, GenericRepository<LeopardType> typeRepo)
        {
            _repo = repo;
            _typeRepo = typeRepo;
        }

        public async Task<List<ProfileResponse>> GetAllHandbagsAsync()
        {
            try
            {
                var handbags = await _repo.GetAllAsync(includeProperties: "Brand");
                var response = handbags
                    .Select(h => new ProfileResponse
                    {
                        LeopardProfileId = h.LeopardProfileId,
                        LeopardTypeId = h.LeopardTypeId,
                        LeopardName = h.LeopardName,
                        LeopardTypeName = h.LeopardType?.LeopardTypeName,
                        Weight = h.Weight,
                        Characteristics = h.Characteristics,
                        CareNeeds = h.CareNeeds,
                        ModifiedDate = h.ModifiedDate,
                    })
                    .ToList();

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<LeopardProfile> GetProfileByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return null;
                return await _repo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ProfileResponse> CreateProfileAsync(LeopardCreateRequest request)
        {
            try
            {
                if (request.LeopardTypeId != 0)
                {
                    var brand = await _typeRepo.GetByIdAsync(request.LeopardTypeId);
                    if (brand == null)
                    {
                        return null;
                    }
                }

                var profile = new LeopardProfile
                {
                    LeopardProfileId = request.LeopardProfileId,
                    LeopardName = request.LeopardName,
                    Weight = request.Weight,
                    Characteristics = request.Characteristics,
                    CareNeeds = request.CareNeeds,
                    LeopardTypeId = request.LeopardTypeId,
                    ModifiedDate = request.ModifiedDate,
                };

                var createdHandbag = await _repo.InsertAsync(profile);

                var response = new ProfileResponse
                {
                    LeopardProfileId = request.LeopardProfileId,
                    LeopardTypeId = request.LeopardTypeId,
                    LeopardName = request.LeopardName,
                    Weight = request.Weight,
                    Characteristics = request.Characteristics,
                    CareNeeds = request.CareNeeds,
                    ModifiedDate = request.ModifiedDate,
                };

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProfileResponse> UpdateProfileAsync(int id, LeopardUpdateRequest request)
        {
            try
            {
                var existingProfile = await _repo.GetByIdAsync(id);
                if (existingProfile == null || existingProfile.LeopardProfileId != id)
                    return null;

                // Validation
                if (request.LeopardTypeId != 0)
                {
                    var brand = await _typeRepo.GetByIdAsync(request.LeopardTypeId);
                    if (brand == null)
                    {
                        return null;
                    }
                }

                existingProfile.LeopardTypeId = request.LeopardTypeId;
                existingProfile.LeopardName = request.LeopardName;
                existingProfile.Weight = request.Weight;
                existingProfile.Characteristics = request.Characteristics;
                existingProfile.CareNeeds = request.CareNeeds;

                var updatedProfile = await _repo.UpdateAsync(existingProfile);
                var response = new ProfileResponse
                {
                    LeopardProfileId = updatedProfile.LeopardProfileId,
                    LeopardTypeId = updatedProfile.LeopardTypeId,
                    LeopardTypeName = updatedProfile.LeopardType?.LeopardTypeName,
                    Weight = updatedProfile.Weight,
                    Characteristics = updatedProfile.Characteristics,
                    CareNeeds = updatedProfile.CareNeeds,
                    ModifiedDate = updatedProfile.ModifiedDate,
                };

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteProfileAsync(int id)
        {
            try
            {
                var handbag = await _repo.GetByIdAsync(id);
                if (handbag == null)
                {
                    return false;
                }

                await _repo.RemoveAsync(handbag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<LeopardProfile> GetProfilesQueryableByQuery(ProfileQueryRequest request)
        {
            try
            {
                var query = _repo.GetQueryable(includeProperties: "LeopardType");

                if (!string.IsNullOrEmpty(request.LeopardName))
                {
                    query = query.Where(x => x.LeopardName.ToLower().Contains(request.LeopardName.ToLower()));
                }


                if (request.Weight > 15)
                {
                    query = query.Where(x => x.Weight == request.Weight);
                }

                return query;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
