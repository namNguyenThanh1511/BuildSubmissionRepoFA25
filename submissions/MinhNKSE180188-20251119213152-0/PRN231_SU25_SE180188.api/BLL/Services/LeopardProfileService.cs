using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _leopardProfileRepository;

        public LeopardProfileService(ILeopardProfileRepository leopardProfileRepository)
        {
            _leopardProfileRepository = leopardProfileRepository;
        }

        public async Task<List<LeopardProfile>> GetAllAsync() => await _leopardProfileRepository.GetAllAsync();

        public async Task<LeopardProfile> GetByIdAsync(int id) => await _leopardProfileRepository.GetByIdAsync(id);

        public IQueryable<LeopardProfile> Search(string? modelName, double? weight) => _leopardProfileRepository.Search(modelName, weight);

        public async Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> AddAsync(LeopardProfile leopardProfile)
        {
            if (string.IsNullOrWhiteSpace(leopardProfile.LeopardName) ||
                !System.Text.RegularExpressions.Regex.IsMatch(leopardProfile.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return (false, "HB40001", "LeopardName is required or invalid");

            if (leopardProfile.Weight <= 15)
                return (false, "HB40001", "Weight must be greater than 15");

            await _leopardProfileRepository.AddAsync(leopardProfile);
            return (true, null, null);
        }

        public async Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> UpdateAsync(LeopardProfile leopardProfile)
        {
            var existing = await _leopardProfileRepository.GetByIdAsync(leopardProfile.LeopardProfileId);
            if (existing == null)
                return (false, "HB40401", "LeopardProfile not found");

            if (string.IsNullOrWhiteSpace(leopardProfile.LeopardName) ||
                !System.Text.RegularExpressions.Regex.IsMatch(leopardProfile.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return (false, "HB40001", "LeopardName is required or invalid");

            if (leopardProfile.Weight <= 15)
                return (false, "HB40001", "Weight must be greater than 15");

            await _leopardProfileRepository.UpdateAsync(leopardProfile);
            return (true, null, null);
        }

        public async Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> DeleteAsync(int id)
        {
            var existing = await _leopardProfileRepository.GetByIdAsync(id);
            if (existing == null)
                return (false, "HB40401", "LeopardProfile not found");

            await _leopardProfileRepository.DeleteAsync(id);
            return (true, null, null);
        }
    }
}
