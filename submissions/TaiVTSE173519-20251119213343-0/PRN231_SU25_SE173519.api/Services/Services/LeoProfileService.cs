using Repositories.Entity;
using Repositories.Interfaces;
using Services.Exception;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LeoProfileService : ILeoProfileService
    {
        private readonly IObjRepository _repo;
        private readonly ISubTableRepository _subTableRepo;

        public LeoProfileService(IObjRepository repo, ISubTableRepository subTableRepo)
        {
            _repo = repo;
            _subTableRepo = subTableRepo;
        }

        public async Task Create(LeoCreateModel model)
        {
            if (model == null)
                throw new AppException("HB40001", "Missing/invalid input");

            if (string.IsNullOrWhiteSpace(model.LeopardName))
                throw new AppException("HB40001", "modelName is required");

            model.Validate();

            if (!await _subTableRepo.ExistsAsync(model.LeopardTypeId))
                throw new AppException("HB40401", "Resource not found");

            /*var all = await _repo.GetAllAsync();
            var nextId = all.Any() ? all.Max(x => x.LeopardProfileId) + 1 : 1;*/

            var entity = new LeopardProfile
            {
                /*LeopardProfileId = nextId,*/
                LeopardName = model.LeopardName,
                Weight = model.Weight,
                Characteristics = model.Characteristics,
                CareNeeds = model.CareNeeds,
                ModifiedDate = model.ModifiedDate,
                LeopardTypeId = model.LeopardTypeId,
            };

            await _repo.AddAsync(entity);
        }

        public async Task<List<LeoResponse>> Get(int pageIndex, int pageSize)
        {
            var paged = await _repo.GetPagedAsync(pageIndex, pageSize);
            if (!paged.Any())
                throw new AppException("HB40401", "Resource not found", 404);

            return paged.Select(x => new LeoResponse
            {
                LeopardProfileId = x.LeopardProfileId,
                LeopardName = x.LeopardName,
                Weight = x.Weight,
                Characteristics = x.Characteristics,
                CareNeeds = x.CareNeeds,
                ModifiedDate = x.ModifiedDate,
                LeopardTypeId = x.LeopardTypeId
            }).ToList();
        }

        public async Task<LeoResponse> Get(int id)
        {
            if (id <= 0)
                throw new AppException("HB40001", "Missing/invalid input");

            var obj = await _repo.GetByIdAsync(id);
            if (obj == null)
                throw new AppException("HB40401", "Resource not found", 404);

            return new LeoResponse
            {
                LeopardProfileId = obj.LeopardProfileId,
                LeopardName = obj.LeopardName,
                Weight = obj.Weight,
                Characteristics = obj.Characteristics,
                CareNeeds = obj.CareNeeds,
                ModifiedDate = obj.ModifiedDate,
                LeopardTypeId = obj.LeopardTypeId
            };
        }

        public async Task<List<LeoResponse>> Search(string? name, double? weight, int pageIndex, int pageSize)
        {
            var paged = await _repo.SearchPagedAsync(name, weight, pageIndex, pageSize);
            if (!paged.Any())
                throw new AppException("HB40401", "Resource not found", 404);

            return paged.Select(obj => new LeoResponse
            {
                LeopardProfileId = obj.LeopardProfileId,
                LeopardName = obj.LeopardName,
                Weight = obj.Weight,
                Characteristics = obj.Characteristics,
                CareNeeds = obj.CareNeeds,
                ModifiedDate = obj.ModifiedDate,
                LeopardTypeId = obj.LeopardTypeId
            }).ToList();
        }

        public async Task Update(int id, LeoCreateModel model)
        {
            if (id <= 0 || model == null)
                throw new AppException("HB40001", "Missing/invalid input");

            if (string.IsNullOrWhiteSpace(model.LeopardName))
                throw new AppException("HB40001", "modelName is required");

            model.Validate();

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new AppException("HB40401", "Resource not found", 404);

            if (!await _subTableRepo.ExistsAsync(model.LeopardTypeId))
                throw new AppException("HB40401", "Resource not found", 404);

            entity.LeopardName = model.LeopardName;
            entity.Weight = model.Weight;
            entity.Characteristics = model.Characteristics;
            entity.Weight = model.Weight;
            entity.CareNeeds = model.CareNeeds;
            entity.ModifiedDate = model.ModifiedDate;
            entity.LeopardTypeId = model.LeopardTypeId;

            await _repo.UpdateAsync(entity);
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new AppException("HB40001", "Missing/invalid input");

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new AppException("HB40401", "Resource not found", 404);

            await _repo.DeleteAsync(entity);
        }
    }
}
