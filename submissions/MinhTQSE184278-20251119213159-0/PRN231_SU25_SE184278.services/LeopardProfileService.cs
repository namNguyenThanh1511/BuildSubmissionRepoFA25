using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE184278.repositories;
using PRN231_SU25_SE184278.services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileReposiotry _repo;

        public LeopardProfileService(ILeopardProfileReposiotry repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LeopardProfileDto>> GetAllAsync()
        {
            var data = await _repo.Query()
                .Select(l => new LeopardProfileDto
                {
                    LeopardProfileId =l.LeopardProfileId,
                    LeopardTypeId = l.LeopardTypeId,
                    LeopardName = l.LeopardName,
                    Weight = l.Weight,
                    Characteristics= l.Characteristics,
                    CareNeeds = l.CareNeeds,
                    ModifiedDate = l.ModifiedDate
                }).ToListAsync();

            return data;
        }

        public async Task<LeopardProfileDto?> GetByIdAsync(int id)
        {
            var l = await _repo.GetByIdAsync(id);
            if (l == null) return null;

            return new LeopardProfileDto
            {
                LeopardProfileId = l.LeopardProfileId,
                LeopardTypeId = l.LeopardTypeId,
                LeopardName = l.LeopardName,
                Weight = l.Weight,
                Characteristics = l.Characteristics,
                CareNeeds = l.CareNeeds,
                ModifiedDate = l.ModifiedDate
            };
        }

        private ErrorResponseDto? ValidateHandbagInput(LeopardProfileCreateDto dto)
        {
            if (!Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
            {
                return new ErrorResponseDto
                {
                    ErrorCode = "HB40001",
                    Message = "LeopardName is invalid"
                };
            }

            if (dto.Weight <= 15)
            {
                return new ErrorResponseDto
                {
                    ErrorCode = "HB40001",
                    Message = "Weight must be greater than 15"
                };
            }
            return null; // No error
        }

        public async Task<(bool IsSuccess, ErrorResponseDto? Error)> CreateAsync(LeopardProfileCreateDto dto)
        {

            var validationError = ValidateHandbagInput(dto);
            if (validationError != null)
                return (false, validationError);


            var leopardProfile = new LeopardProfile
            {
                LeopardProfileId = dto.LeopardProfileId,
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate
            };

            await _repo.CreateAsync(leopardProfile);
            return (true, null);
        }


        public async Task<(bool IsSuccess, ErrorResponseDto? Error)> UpdateAsync(int id, LeopardProfileCreateDto dto)
        {
            var leopardProfile = await _repo.GetByIdAsync(id);
            if (leopardProfile == null)
            {
                return (false, new ErrorResponseDto { ErrorCode = "HB40401", Message = "LeopardProfile not found" });
            }
            var validationError = ValidateHandbagInput(dto);
            if (validationError != null)
                return (false, validationError);

            leopardProfile.LeopardProfileId = dto.LeopardProfileId;
            leopardProfile.LeopardTypeId = dto.LeopardTypeId;
            leopardProfile.LeopardName = dto.LeopardName;
            leopardProfile.Weight = dto.Weight;
            leopardProfile.Characteristics = dto.Characteristics;
            leopardProfile.CareNeeds = dto.CareNeeds;
            leopardProfile.ModifiedDate = dto.ModifiedDate;

            await _repo.UpdateAsync(leopardProfile);
            return (true, null);
        }

        public async Task<(bool IsSuccess, ErrorResponseDto? Error)> DeleteAsync(int id)
        {
            var leopardProfile = await _repo.GetByIdAsync(id);
            if (leopardProfile == null)
            {
                return (false, new ErrorResponseDto { ErrorCode = "HB40401", Message = "LeopardProfile not found" });
            }

            await _repo.DeleteAsync(leopardProfile);
            return (true, null);
        }

        public async Task<List<LeopardProfileDto>> SearchAsync(string? LeopardName, double? Weight)
        {
            var query = _repo.Query();

            if (!string.IsNullOrEmpty(LeopardName))
            {
                query = query.Where(h => h.LeopardName.Contains(LeopardName));
            }

            if (!(Weight==0))
            {
                query = query.Where(h => h.Weight != null && h.Weight == Weight);
            }

            var list = await query.Select(l => new LeopardProfileDto
            {
                LeopardProfileId = l.LeopardProfileId,
                LeopardTypeId = l.LeopardTypeId,
                LeopardName = l.LeopardName,
                Weight = l.Weight,
                Characteristics = l.Characteristics,
                CareNeeds = l.CareNeeds,
                ModifiedDate = l.ModifiedDate
            }).ToListAsync();

            return list;
        }
    }
}

