using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE181580.BLL.DTOs;
using PRN231_SU25_SE181580.BLL.Interfaces;
using PRN231_SU25_SE181580.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN231_SU25_SE181580.BLL.Implementations {
    public class LeopardProfileService: ILeoPardProfileService {
        private readonly SU25LeopardDBContext _context;
        private static readonly Regex _namePattern = new(
            @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$",
            RegexOptions.Compiled);

        public LeopardProfileService(SU25LeopardDBContext context)
            => _context = context;

        public async Task<IEnumerable<LeopardProfileDTO>> GetAllAsync()
            => await _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .Select(h => ToDto(h))
                .ToListAsync();

        public async Task<LeopardProfileDTO?> GetByIdAsync(int id) {
            var h = await _context.LeopardProfiles
                .Include(x => x.LeopardType)
                .FirstOrDefaultAsync(x => x.LeopardProfileId == id);
            return h is null ? null : ToDto(h);
        }

        public async Task<LeopardProfileDTO> CreateAsync(LeopardProfileDTO dto) {
            if (string.IsNullOrWhiteSpace(dto.LeopardName))
                throw new ArgumentException("modelName is required");

            if (!_namePattern.IsMatch(dto.LeopardName))
                throw new ArgumentException("modelName is invalid");

            if (dto.Weight <= 15)
                throw new ArgumentException("Weight must be greater than 15");

            var type = await _context.LeopardProfiles.FindAsync(dto.LeopardTypeId);
            if (type == null)
                throw new ArgumentException("type is invalid");

            var entity = new LeopardProfile {
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
            };

            _context.LeopardProfiles.Add(entity);
            await _context.SaveChangesAsync();

            return new LeopardProfileDTO {
                LeopardProfileId = entity.LeopardProfileId,
                LeopardTypeId = entity.LeopardTypeId,
                LeopardName = entity.LeopardName,
                Weight = entity.Weight,
                Characteristics = entity.Characteristics,
                CareNeeds = entity.CareNeeds,
                ModifiedDate = entity.ModifiedDate,
            };
        }


        public async Task<LeopardProfileDTO?> UpdateAsync(int id, LeopardProfileDTO dto) {
            var h = await _context.LeopardProfiles.FindAsync(id);
            if (h is null) return null;

            if (!_namePattern.IsMatch(dto.LeopardName))
                throw new ArgumentException("LeopardName is invalid", nameof(dto.LeopardName));
            if (dto.Weight <= 15)
                throw new ArgumentException("Weight must be > 15");

            h.LeopardTypeId = dto.LeopardTypeId;
            h.LeopardName = dto.LeopardName;
            h.Weight = dto.Weight;
            h.Characteristics = dto.Characteristics;
            h.CareNeeds = dto.CareNeeds;
            h.ModifiedDate = dto.ModifiedDate;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id) {
            var h = await _context.LeopardProfiles.FindAsync(id);
            if (h is null) return false;
            _context.LeopardProfiles.Remove(h);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<IGrouping<string, LeopardProfileDTO>>> SearchAsync(string? leopardName, double? weight) {
            var q = _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(leopardName))
                q = q.Where(h => h.LeopardName.Contains(leopardName!));

            var list = await q.Select(h => ToDto(h)).ToListAsync();
            return list.GroupBy(h => h.Characteristics);
        }

        private static LeopardProfileDTO ToDto(LeopardProfile h) => new() {
            LeopardProfileId = h.LeopardProfileId,
            LeopardTypeId = h.LeopardTypeId,
            LeopardName = h.LeopardName,
            Weight = h.Weight,
            Characteristics = h.Characteristics,
            CareNeeds = h.CareNeeds,
            ModifiedDate = h.ModifiedDate,
        };

        public IQueryable<LeopardProfileDTO> AsQueryable() {
            return _context.LeopardProfiles
                .AsNoTracking()
                .Include(h => h.LeopardType)
                .Select(h => new LeopardProfileDTO {
                    LeopardProfileId = h.LeopardProfileId,
                    LeopardTypeId = h.LeopardTypeId,
                    LeopardName = h.LeopardName,
                    Weight = h.Weight,
                    Characteristics = h.Characteristics,
                    CareNeeds = h.CareNeeds,
                    ModifiedDate = h.ModifiedDate,
                });
        }


        public class ErrorResponse {
            public string ErrorCode { get; }
            public string Message { get; }

            public ErrorResponse(string errorCode, string message) {
                ErrorCode = errorCode;
                Message = message;
            }
        }
    }


}
