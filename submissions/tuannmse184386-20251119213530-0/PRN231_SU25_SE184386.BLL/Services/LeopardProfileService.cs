using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184386.DAL.ModelExtensions;
using PRN231_SU25_SE184386.DAL.Models;
using PRN231_SU25_SE184386.DAL.Repositories;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE184386.BLL.Services
{
    public class LeopardProfileService
    {
        private readonly GenericRepository<LeopardProfile> _genericRepository;
        private readonly GenericRepository<LeopardType> _LeopardTypeRepository;

        public LeopardProfileService(GenericRepository<LeopardProfile> genericRepository, GenericRepository<LeopardType> LeopardTypeRepository)
        {
            _genericRepository = genericRepository;
            _LeopardTypeRepository = LeopardTypeRepository;
        }

        public async Task<ApiResponse<List<GetLeopardProfileResponse>>> GetAllLeopardProfilesAsync()
        {
            var response = new ApiResponse<List<GetLeopardProfileResponse>>();

            var LeopardProfiles = await _genericRepository.FindWithIncludeAsync(
                include: query => query.Include(x => x.LeopardType));

            response.Data = LeopardProfiles.Select(LeopardProfile => new GetLeopardProfileResponse
            {
                LeopardProfileId = LeopardProfile.LeopardProfileId,
                LeopardName = LeopardProfile.LeopardName,
                Weight = LeopardProfile.Weight,
                Characteristics = LeopardProfile.Characteristics,
                CareNeeds = LeopardProfile.CareNeeds,
                ModifiedDate = LeopardProfile.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = LeopardProfile.LeopardType.LeopardTypeId,
                    LeopardTypeName = LeopardProfile.LeopardType.LeopardTypeName,
                    Origin = LeopardProfile.LeopardType.Origin,
                    Description = LeopardProfile.LeopardType.Description
                }
            }).ToList();

            response.Success = true;
            return response;
        }

        public async Task<ApiResponse<GetLeopardProfileResponse>> GetLeopardProfileByIdAsync(int id)
        {
            var response = new ApiResponse<GetLeopardProfileResponse>();
            var detailError = new DetailError();

            var LeopardProfiles = await _genericRepository.FindWithIncludeAsync(
                predicate: query => query.LeopardProfileId == id,
                include: query => query.Include(x => x.LeopardType));

            if (!LeopardProfiles.Any())
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "Resource not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            response.Success = true;
            response.Data = LeopardProfiles.Select(LeopardProfile => new GetLeopardProfileResponse
            {
                LeopardProfileId = LeopardProfile.LeopardProfileId,
                LeopardName = LeopardProfile.LeopardName,
                Weight = LeopardProfile.Weight,
                Characteristics = LeopardProfile.Characteristics,
                CareNeeds = LeopardProfile.CareNeeds,
                ModifiedDate = LeopardProfile.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = LeopardProfile.LeopardType.LeopardTypeId,
                    LeopardTypeName = LeopardProfile.LeopardType.LeopardTypeName,
                    Origin = LeopardProfile.LeopardType.Origin,
                    Description = LeopardProfile.LeopardType.Description
                }
            }).FirstOrDefault();

            return response;
        }

        private DetailError ValidateLeopardProfileDTO(string LeopardName, double Weight)
        {
            var detailError = new DetailError();
            var regex = new Regex("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

            if (!regex.IsMatch(LeopardName))
            {
                detailError.ErrorCode = "HB40001";
                detailError.Message = "LeopardName is required to match regex";
                return detailError;
            }

            if (Weight <= 15)
            {
                detailError.ErrorCode = "HB40001";
                detailError.Message = "Weight must be greater than 15";
                return detailError;
            }



            return detailError;
        }

        public async Task<ApiResponse<string>> CreateLeopardProfileAsync(LeopardProfileDTO request)
        {
            var response = new ApiResponse<string>();
            var detailError = ValidateLeopardProfileDTO(request.LeopardName, request.Weight);

            if (!string.IsNullOrEmpty(detailError.ErrorCode))
            {
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            var LeopardProfile = new LeopardProfile
            {
                LeopardTypeId = request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                ModifiedDate = request.ModifiedDate,
            };


            _ = await _genericRepository.CreateAsync(LeopardProfile);
            _ = await _genericRepository.SaveAsync();

            response.Success = true;
            return response;
        }

        public async Task<ApiResponse<string>> UpdateLeopardProfileAsync(int id, LeopardProfileDTO request)
        {
            var response = new ApiResponse<string>();
            var detailError = new DetailError();

            var LeopardProfileInDb = await _genericRepository.GetByIdAsync(id);
            if (LeopardProfileInDb == null)
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "Resource not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            detailError = ValidateLeopardProfileDTO(request.LeopardName, request.Weight);
            if (!string.IsNullOrEmpty(detailError.ErrorCode))
            {
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            LeopardProfileInDb.LeopardTypeId = request.LeopardTypeId;
            LeopardProfileInDb.LeopardName = request.LeopardName;
            LeopardProfileInDb.Weight = request.Weight;
            LeopardProfileInDb.Characteristics = request.Characteristics;
            LeopardProfileInDb.CareNeeds = request.CareNeeds;
            LeopardProfileInDb.ModifiedDate = request.ModifiedDate;

            _ = await _genericRepository.UpdateAsync(LeopardProfileInDb);
            _ = await _genericRepository.SaveAsync();

            response.Success = true;
            return response;
        }

        public async Task<ApiResponse<string>> DeleteLeopardProfileByIdAsync(int id)
        {
            var response = new ApiResponse<string>();
            var detailError = new DetailError();

            var LeopardProfileInDb = await _genericRepository.GetByIdAsync(id);
            if (LeopardProfileInDb == null)
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "Resource not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            _ = await _genericRepository.RemoveAsync(LeopardProfileInDb);
            _ = await _genericRepository.SaveAsync();

            response.Success = true;
            return response;
        }

        public async Task<List<GetLeopardProfileResponse>> ListAllAsync()
        {
            return (await _genericRepository.FindWithIncludeAsync(include: query => query.Include(x => x.LeopardType))).Select(LeopardProfile => new GetLeopardProfileResponse
            {
                LeopardProfileId = LeopardProfile.LeopardProfileId,
                LeopardName = LeopardProfile.LeopardName,
                Weight = LeopardProfile.Weight,
                Characteristics = LeopardProfile.Characteristics,
                CareNeeds = LeopardProfile.CareNeeds,
                ModifiedDate = LeopardProfile.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = LeopardProfile.LeopardType.LeopardTypeId,
                    LeopardTypeName = LeopardProfile.LeopardType.LeopardTypeName,
                    Origin = LeopardProfile.LeopardType.Origin,
                    Description = LeopardProfile.LeopardType.Description
                }
            }).ToList();
        }
    }
}
