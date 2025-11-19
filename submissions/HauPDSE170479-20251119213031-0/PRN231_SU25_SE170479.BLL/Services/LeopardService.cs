using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE170479.DAL.DTOs;
using PRN231_SU25_SE170479.DAL.ModelExtensions;
using PRN231_SU25_SE170479.DAL.Models;
using PRN231_SU25_SE170479.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE170479.BLL.Services
{
    public class LeopardService : ILeopardService
    {
        private readonly GenericRepository<LeopardProfile> _repo;

        public LeopardService(GenericRepository<LeopardProfile> repo)
        {
            _repo = repo;
        }

        public async Task<Result<List<GetLeopardProfileResponse>>> GetAllAsync()
        {
            var result = await _repo.FindWithIncludeAsync(
        include: query => query.Include(x => x.LeopardType));

            var data = result.Select(MapToResponse).ToList();
            return Result<List<GetLeopardProfileResponse>>.Ok(data);
        }

        public async Task<Result<GetLeopardProfileResponse>> GetByIdAsync(int id)
        {
            var result = (await _repo.FindWithIncludeAsync(
                a => a.LeopardProfileId == id,
                q => q.Include(x => x.LeopardType))).FirstOrDefault();

            if (result == null)
                return Errors.NotFound<GetLeopardProfileResponse>();

            return Result<GetLeopardProfileResponse>.Ok(MapToResponse(result));
        }

        public async Task<Result<string>> CreateAsync(LeopardDTO dto)
        {
            var validation = LeopardValidator.Validate(dto);
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
                ModifiedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))
            };

            await _repo.CreateAsync(newModel);
            return Result<string>.Ok("Created successfully");
        }

        public async Task<Result<string>> UpdateAsync(int id, LeopardDTO dto)
        {
            var existingModel = await _repo.GetByIdAsync(id);
            if (existingModel == null)
                return Errors.NotFound<string>();

            var validation = LeopardValidator.Validate(dto);
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

        public async Task<List<GetLeopardProfileResponse>> ListAllAsync()
        {
            var result = await _repo.FindWithIncludeAsync(include: query => query.Include(x => x.LeopardType));
            return result.Select(MapToResponse).ToList();
        }

        private GetLeopardProfileResponse MapToResponse(LeopardProfile model)
        {
            return new GetLeopardProfileResponse
            {
                LeopardProfileId = model.LeopardProfileId,
                LeopardName = model.LeopardName,
                Weight = model.Weight,
                Characteristics = model.Characteristics,
                CareNeeds = model.CareNeeds,
                ModifiedDate = model.ModifiedDate,
                LeopardType = new GetLeopardTypeResponse
                {
                    LeopardTypeId = model.LeopardType.LeopardTypeId,
                    LeopardTypeName = model.LeopardType.LeopardTypeName,
                    Origin = model.LeopardType.Origin,
                    Description = model.LeopardType.Description
                }
            };
        }
    }
}
