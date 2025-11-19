using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repo;
        private const string NamePattern = @"^([A-Z0-9][a-zA-Z0-9]*\s)*([A-Z0-9][a-zA-Z0-9]*)$";

        public LeopardProfileService(ILeopardProfileRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<LeopardProfile>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<LeopardProfile?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<LeopardProfile> CreateAsync(LeopardProfile entity)
        {
            Validate(entity);
            return await _repo.AddAsync(entity);
        }

        public async Task<LeopardProfile> UpdateAsync(int id, LeopardProfile entity)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null)
                throw new KeyNotFoundException($"LeopardProfile with id={id} not found");

            Validate(entity);

            existing.LeopardName = entity.LeopardName;
            existing.Weight = entity.Weight;
            existing.Characteristics = entity.Characteristics;
            existing.CareNeeds = entity.CareNeeds;
            existing.ModifiedDate = entity.ModifiedDate;
            existing.LeopardTypeId = entity.LeopardTypeId;

            await _repo.UpdateAsync(existing);
            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null)
                throw new KeyNotFoundException($"LeopardProfile with id={id} not found");

            await _repo.DeleteAsync(id);
        }

        public Task<IEnumerable<LeopardProfile>> SearchAsync(string? leopardName, double? weight)
            => _repo.SearchAsync(leopardName, weight);

        private void Validate(LeopardProfile e)
        {
            if (string.IsNullOrWhiteSpace(e.LeopardName)
                || !Regex.IsMatch(e.LeopardName, NamePattern))
            {
                throw new ArgumentException("LeopardName is invalid (must match pattern).");
            }

            if (e.Weight <= 15)
            {
                throw new ArgumentException("Weight must be greater than 15.");
            }
        }
    }
}
