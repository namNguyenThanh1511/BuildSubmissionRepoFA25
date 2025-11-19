using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184437.DAL.ModelExtensions;
using PRN231_SU25_SE184437.DAL.Models;
using PRN231_SU25_SE184437.DAL.Repositories;

namespace PRN231_SU25_SE184437.BLL.Services
{
    public class LeopardProfileService
    {
        private readonly GenericRepository<LeopardProfile> _genericRepository;
        private readonly GenericRepository<LeopardType> _leopardTypeRepository;

        public LeopardProfileService(GenericRepository<LeopardProfile> genericRepository, GenericRepository<LeopardType> leopardTypeRepository)
        {
            _genericRepository = genericRepository;
            _leopardTypeRepository = leopardTypeRepository;
        }

        public async Task<ApiResponse<List<GetLeopardProfileResponse>>> GetAllLeopardProfilesAsync()
        {
            var response = new ApiResponse<List<GetLeopardProfileResponse>>();

            var leopardProfiles = await _genericRepository.FindWithIncludeAsync(
                include: query => query.Include(x => x.LeopardType));

            response.Data = leopardProfiles.Select(leopardProfile => new GetLeopardProfileResponse
            {
                LeopardProfileId = leopardProfile.LeopardProfileId,
                LeopardTypeId = leopardProfile.LeopardTypeId,
                LeopardName = leopardProfile.LeopardName,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = leopardProfile.LeopardType.LeopardTypeId,
                    LeopardTypeName = leopardProfile.LeopardType.LeopardTypeName,
                    Origin = leopardProfile.LeopardType.Origin,
                    Description = leopardProfile.LeopardType.Description
                }
            }).ToList();

            response.Success = true;
            return response;
        }

        public async Task<ApiResponse<GetLeopardProfileResponse>> GetLeopardProfileByIdAsync(int id)
        {
            var response = new ApiResponse<GetLeopardProfileResponse>();
            var detailError = new DetailError();

            var leopardProfiles = await _genericRepository.FindWithIncludeAsync(
                predicate: query => query.LeopardProfileId == id,
                include: query => query.Include(x => x.LeopardType));

            if (!leopardProfiles.Any())
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "LeopardProfile not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            response.Success = true;
            response.Data = leopardProfiles.Select(leopardProfile => new GetLeopardProfileResponse
            {
                LeopardProfileId = leopardProfile.LeopardProfileId,
                LeopardTypeId = leopardProfile.LeopardTypeId,
                LeopardName = leopardProfile.LeopardName,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = leopardProfile.LeopardType.LeopardTypeId,
                    LeopardTypeName = leopardProfile.LeopardType.LeopardTypeName,
                    Origin = leopardProfile.LeopardType.Origin,
                    Description = leopardProfile.LeopardType.Description
                }
            }).FirstOrDefault();

            return response;
        }

        private DetailError ValidateLeopardProfileDTO(string leopardName, double weight)
        {
            var detailError = new DetailError();
            var regex = new Regex("^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

            if (!regex.IsMatch(leopardName))
            {
                detailError.ErrorCode = "HB40001";
                detailError.Message = "Leopard Name is required";
                return detailError;
            }

            if (weight <= 15)
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

            var leopardTypeExists = await _leopardTypeRepository.GetByIdAsync(request.LeopardTypeId);
            if (leopardTypeExists == null)
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "LeopardType not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            var leopardProfile = new LeopardProfile
            {
                LeopardTypeId = request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                ModifiedDate = DateTime.Now,
            };

            _ = await _genericRepository.CreateAsync(leopardProfile);
            _ = await _genericRepository.SaveAsync();

            response.Success = true;
            return response;
        }

        public async Task<ApiResponse<string>> UpdateLeopardProfileAsync(int id, LeopardProfileDTO request)
        {
            var response = new ApiResponse<string>();
            var detailError = new DetailError();

            var leopardProfileInDb = await _genericRepository.GetByIdAsync(id);
            if (leopardProfileInDb == null)
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "LeopardProfile not found";
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

            var leopardTypeExists = await _leopardTypeRepository.GetByIdAsync(request.LeopardTypeId);
            if (leopardTypeExists == null)
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "LeopardType not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            leopardProfileInDb.LeopardTypeId = request.LeopardTypeId;
            leopardProfileInDb.LeopardName = request.LeopardName;
            leopardProfileInDb.Weight = request.Weight;
            leopardProfileInDb.Characteristics = request.Characteristics;
            leopardProfileInDb.CareNeeds = request.CareNeeds;
            leopardProfileInDb.ModifiedDate = DateTime.Now;

            _ = await _genericRepository.UpdateAsync(leopardProfileInDb);
            _ = await _genericRepository.SaveAsync();

            response.Success = true;
            return response;
        }

        public async Task<ApiResponse<string>> DeleteLeopardProfileByIdAsync(int id)
        {
            var response = new ApiResponse<string>();
            var detailError = new DetailError();

            var leopardProfileInDb = await _genericRepository.GetByIdAsync(id);
            if (leopardProfileInDb == null)
            {
                detailError.ErrorCode = "HB40401";
                detailError.Message = "LeopardProfile not found";
                response.DetailError = detailError;
                response.Success = false;
                return response;
            }

            _ = await _genericRepository.RemoveAsync(leopardProfileInDb);
            _ = await _genericRepository.SaveAsync();

            response.Success = true;
            return response;
        }

        public async Task<List<GetLeopardProfileResponse>> ListAllAsync()
        {
            return (await _genericRepository.FindWithIncludeAsync(include: query => query.Include(x => x.LeopardType))).Select(leopardProfile => new GetLeopardProfileResponse
            {
                LeopardProfileId = leopardProfile.LeopardProfileId,
                LeopardTypeId = leopardProfile.LeopardTypeId,
                LeopardName = leopardProfile.LeopardName,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = leopardProfile.LeopardType.LeopardTypeId,
                    LeopardTypeName = leopardProfile.LeopardType.LeopardTypeName,
                    Origin = leopardProfile.LeopardType.Origin,
                    Description = leopardProfile.LeopardType.Description
                }
            }).ToList();
        }
    }
}
