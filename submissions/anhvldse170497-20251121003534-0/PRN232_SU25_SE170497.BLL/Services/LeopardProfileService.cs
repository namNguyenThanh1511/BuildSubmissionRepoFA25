using Microsoft.EntityFrameworkCore;
using PRN232_SU25_SE170497.DAL.DTOs;
using PRN232_SU25_SE170497.DAL.ModelExtensions;
using PRN232_SU25_SE170497.DAL.Models;
using PRN232_SU25_SE170497.DAL.Repositories;

namespace BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly GenericRepository<LeopardProfile> _repo;

        public LeopardProfileService(GenericRepository<LeopardProfile> repo)
        {
            _repo = repo;
        }

        public async Task<Result<List<GetLeopardProfileRespone>>> GetAllAsync()
        {
            var result = await _repo.FindWithIncludeAsync(
        include: query => query.Include(x => x.LeopardType));

            var data = result.Select(MapToResponse).ToList();
            return Result<List<GetLeopardProfileRespone>>.Ok(data);
        }

        public async Task<Result<GetLeopardProfileRespone>> GetByIdAsync(int id)
        {
            var result = (await _repo.FindWithIncludeAsync(
                a => a.LeopardProfileId == id,
                q => q.Include(x => x.LeopardType))).FirstOrDefault();

            if (result == null)
                return Errors.NotFound<GetLeopardProfileRespone>();

            return Result<GetLeopardProfileRespone>.Ok(MapToResponse(result));
        }

        public async Task<Result<string>> CreateAsync(LeopardProfileDTO dto)
        {
            var validation = LeopardProfileValidator.Validate(dto);
            if (!validation.Success)
                return validation;

            var latestId = await _repo.GetMaxIntPropertyAsync(a => a.LeopardProfileId);
            var nextId = latestId + 1;

            var newModel = new LeopardProfile
            {
                LeopardProfileId = nextId,
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
            };

            await _repo.CreateAsync(newModel);
            return Result<string>.Ok("Created successfully");
        }

        public async Task<Result<string>> UpdateAsync(int id, LeopardProfileDTO dto)
        {
            var existingModel = await _repo.GetByIdAsync(id);
            if (existingModel == null)
                return Errors.NotFound<string>();

            var validation = LeopardProfileValidator.Validate(dto);
            if (!validation.Success)
                return validation;

            existingModel.LeopardTypeId = dto.LeopardTypeId;
            existingModel.LeopardName = dto.LeopardName;
            existingModel.Weight = dto.Weight;
            existingModel.Characteristics = dto.Characteristics;
            existingModel.CareNeeds = dto.CareNeeds;

            await _repo.UpdateAsync(existingModel);
            return Result<string>.Ok("Updated successfully");
        }

        public async Task<Result<string>> DeleteByIdAsync(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (result == null)
                return Errors.NotFound<string>();

            await _repo.RemoveAsync(result);
            return Result<string>.Ok("Deleted successfully");
        }

        public async Task<List<GetLeopardProfileRespone>> ListAllAsync()
        {
            var result = await _repo.FindWithIncludeAsync(include: query => query.Include(x => x.LeopardType));
            return result.Select(MapToResponse).ToList();
        }

        private GetLeopardProfileRespone MapToResponse(LeopardProfile model)
        {
            return new GetLeopardProfileRespone
            {
                LeopardProfileId = model.LeopardProfileId,
                LeopardName = model.LeopardName,
                Weight = model.Weight,
                Characteristics = model.Characteristics,
                CareNeeds = model.CareNeeds,
                LeopardTypeId = model.LeopardTypeId,
                ModifiedDate = model.ModifiedDate,
                LeopardType = new GetLeopardTypeRespone
                {
                    LeopardTypeId = model.LeopardType.LeopardTypeId,
                    LeopardTypeName = model.LeopardType.LeopardTypeName,
                    Origin = model.LeopardType.Origin,
                    Description = model.LeopardType.Description,
                }
            };
        }
    }
}
