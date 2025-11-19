using BLL.DTOs;
using DAL;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repo;

        public LeopardProfileService(ILeopardProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LeopardProfileDto>> GetAllAsync()
        {
            var data = await _repo.Query()
                .Select(h => new LeopardProfileDto
                {
                    LeopardProfileId = h.LeopardProfileId,
                    LeopardTypeId = h.LeopardTypeId,
                    LeopardName = h.LeopardName,
                    Weight = h.Weight,
                    Characteristics = h.Characteristics,
                    CareNeeds = h.CareNeeds,
                    ModifiedDate = h.ModifiedDate
                }).ToListAsync();

            return data;
        }

        public async Task<LeopardProfileDto?> GetByIdAsync(int id)
        {
            var h = await _repo.GetByIdAsync(id);
            if (h == null) return null;

            return new LeopardProfileDto
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate
            };
        }

        private ErrorResponseDto? ValidateLeopardInput(LeopardProfileCreateDto dto)
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
                    Message = "wight must be greater than 15"
                };
            }

            return null;
        }

        public async Task<(bool IsSuccess, ErrorResponseDto? Error)> CreateAsync(LeopardProfileCreateDto dto)
        {

            var validationError = ValidateLeopardInput(dto);
            if (validationError != null)
                return (false, validationError);


            var leopard = new LeopardProfile
            {
                LeopardProfileId = await _repo.GetMaxIdAsync() + 1,
                LeopardName = dto.LeopardName,
                CareNeeds = dto.CareNeeds,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                LeopardTypeId = dto.LeopardTypeId
            };

            await _repo.CreateAsync(leopard);
            return (true, null);
        }


        public async Task<(bool IsSuccess, ErrorResponseDto? Error)> UpdateAsync(int id, LeopardProfileCreateDto dto)
        {
            var leopard = await _repo.GetByIdAsync(id);
            if (leopard == null)
            {
                return (false, new ErrorResponseDto { ErrorCode = "HB40401", Message = "Leopard not found" });
            }
            var validationError = ValidateLeopardInput(dto);
            if (validationError != null)
                return (false, validationError);

            leopard.LeopardName = dto.LeopardName;
            leopard.CareNeeds = dto.CareNeeds;
            leopard.Characteristics = dto.Characteristics;
            leopard.Weight = dto.Weight;
            leopard.LeopardTypeId = dto.LeopardTypeId;

            await _repo.UpdateAsync(leopard);
            return (true, null);
        }

        public async Task<(bool IsSuccess, ErrorResponseDto? Error)> DeleteAsync(int id)
        {
            var handbag = await _repo.GetByIdAsync(id);
            if (handbag == null)
            {
                return (false, new ErrorResponseDto { ErrorCode = "HB40401", Message = "Handbag not found" });
            }

            await _repo.DeleteAsync(handbag);
            return (true, null);
        }

        //public async Task<Dictionary<string, List<HandbagDto>>> SearchAsync(string? modelName, string? material)
        //{
        //    var query = _repo.Query();

        //    if (!string.IsNullOrEmpty(modelName))
        //    {
        //        query = query.Where(h => h.ModelName.Contains(modelName));
        //    }

        //    if (!string.IsNullOrEmpty(material))
        //    {
        //        query = query.Where(h => h.Material != null && h.Material.Contains(material));
        //    }

        //    var list = await query.Select(h => new HandbagDto
        //    {
        //        HandbagId = h.HandbagId,
        //        ModelName = h.ModelName,
        //        Material = h.Material,
        //        Color = h.Color,
        //        Price = h.Price,
        //        Stock = h.Stock,
        //        ReleaseDate = h.ReleaseDate,
        //        BrandName = h.Brand.BrandName,
        //        Country = h.Brand.Country,
        //        FoundedYear = h.Brand.FoundedYear,
        //        Website = h.Brand.Website
        //    }).ToListAsync();

        //    // Group by BrandName
        //    var grouped = list.GroupBy(h => h.BrandName ?? "")
        //        .ToDictionary(g => g.Key, g => g.ToList());

        //    return grouped;
        //}
    }
}
