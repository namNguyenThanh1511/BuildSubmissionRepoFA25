using Repositories.DTOs;
using Repositories.Entities;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardService : ILeopardService
    {
        private readonly ILeopardRepository _repo;

        public LeopardService(ILeopardRepository repository)
        {
            _repo = repository;
        }

        public async Task<IEnumerable<LeopardProfileDTO>> GetAllAsync()
        {
            var leopard = await _repo.GetAllAsync();

            return leopard.Select(h => MapToDTO(h));
        }

        public async Task<LeopardProfileDTO> GetByIdAsync(int id)
        {
            var leopard = await _repo.GetByIdAsync(id);
            return leopard == null ? null : MapToDTO(leopard);
        }

        public async Task<(LeopardProfileDTO Created, string? Error)> CreateAsync(CreateLeopardDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return (null, "HB40001:Missing/Invalid input");

            if (dto.Weight <= 15)
                return (null, "HB40001:Missing/Invalid input");
            //var nextId = await _repo.GetNextIdAsync();
            var leopard = new LeopardProfile
            {
                LeopardProfileId = dto.LeopardProfileId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                CareNeeds = dto.CareNeeds,
                Characteristics = dto.Characteristics,
                LeopardTypeId = dto.LeopardTypeId,
                ModifiedDate = dto.ModifiedDate,
            };

            var created = await _repo.AddAsync(leopard);
            return (MapToDTO(created), null);
        }
        public async Task<(LeopardProfile? Updated, string? Error)> UpdateAsync(int id, CreateLeopardDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return (null, "Leopard not found");

            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                 !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return (null, "HB40001:Missing/Invalid input");

            if (dto.Weight <= 15)
                return (null, "HB40001:Missing/Invalid input");

            existing.LeopardName = dto.LeopardName;
            existing.Weight = dto.Weight;
            existing.CareNeeds = dto.CareNeeds;
            existing.Characteristics = dto.Characteristics;
            existing.LeopardTypeId = dto.LeopardTypeId;

            await _repo.UpdateAsync(existing);
            return (existing, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            return await _repo.DeleteAsync(id);
        }
        private LeopardProfileDTO MapToDTO(LeopardProfile h)
        {
            return new LeopardProfileDTO
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardName = h.LeopardName,
                Characteristics = h.Characteristics,
                LeopardTypeId = h.LeopardTypeId,
                Weight = h.Weight,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate,
                LeopardType = h.LeopardType == null ? null : new LeopardTypeDTO
                {
                    LeopardTypeId = h.LeopardType.LeopardTypeId,
                    Origin = h.LeopardType.Origin,
                    LeopardTypeName = h.LeopardType.LeopardTypeName,
                    Description = h.LeopardType.Description,
                }
            };
        }
    }
}
