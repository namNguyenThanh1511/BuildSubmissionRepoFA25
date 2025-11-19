using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IMainEntityService
    {
        Task<IEnumerable<Object>> GetAllMainEntity();
        Task<Object> GetMainEntityById(int id);
        Task<LeopardProfile> CreateMainEntity(LeopardProfile entity);
        Task<LeopardProfile> UpdateMainEntity(int id, LeopardProfile entity);
        Task<bool> DeleteMainEntity(int id);
        Task<IEnumerable<Object>> SearchMainEntity(string? param1, double? param2);
    }

    public class MainEntityService : IMainEntityService
    {
        private readonly IMainEntityRepo _mainEntityRepo;

        public MainEntityService(IMainEntityRepo mainEntityRepo)
        {
            _mainEntityRepo = mainEntityRepo;
        }

        public async Task<IEnumerable<Object>> GetAllMainEntity()
        {
            var listInclude = await _mainEntityRepo.GetAllAsync(e => e.LeopardType);
            var listWithRelatedInfo = listInclude.Select(e => new
            {
                LeopardProfileId = e.LeopardProfileId,
                LeopardTypeId = e.LeopardTypeId,
                LeopardName = e.LeopardName,
                Weight = e.Weight,
                Characteristics = e.Characteristics,
                CareNeeds = e.CareNeeds,
                ModifiedDate = e.ModifiedDate
            });
            return listWithRelatedInfo;
        }

        public async Task<Object> GetMainEntityById(int id)
        {
            var entity = await _mainEntityRepo.GetByIdAsync(id, e => e.LeopardType);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found");
            }

            return new
            {
                LeopardProfileId = entity.LeopardProfileId,
                LeopardTypeId = entity.LeopardTypeId,
                LeopardName = entity.LeopardName,
                Weight = entity.Weight,
                Characteristics = entity.Characteristics,
                CareNeeds = entity.CareNeeds,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public async Task<LeopardProfile> CreateMainEntity(LeopardProfile entity)
        {
            
            
            return await _mainEntityRepo.CreateAsync(entity);
        }

        public async Task<LeopardProfile> UpdateMainEntity(int id, LeopardProfile entity)
        {
            var existing = await _mainEntityRepo.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found");
            }

            existing.LeopardTypeId = entity.LeopardTypeId;
            existing.LeopardName = entity.LeopardName;
            existing.Weight = entity.Weight;
            existing.Weight = entity.Weight;
            existing.Characteristics = entity.Characteristics;
            existing.ModifiedDate = entity.ModifiedDate;
            return await _mainEntityRepo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteMainEntity(int id)
        {
            return await _mainEntityRepo.RemoveAsync(id);
        }

        public async Task<IEnumerable<Object>> SearchMainEntity(string? leopardname, double? weight)
        {
            var entities = await _mainEntityRepo.GetAllAsync(e => e.LeopardType);

            var filtered = entities.Where(e =>
                (string.IsNullOrEmpty(leopardname) || e.LeopardName.Contains(leopardname, StringComparison.OrdinalIgnoreCase)) &&
                (weight == null || e.Weight == weight)
            );

            return filtered
                          .Select(g => new
                          {
                              LeopardProfileId = g.LeopardProfileId,
                              LeopardTypeId = g.LeopardTypeId,
                              LeopardName = g.LeopardName,
                              Weight = g.Weight,
                              Characteristics = g.Characteristics,
                              CareNeeds = g.CareNeeds,
                              ModifiedDate = g.ModifiedDate
                          });
        }
    }

}
