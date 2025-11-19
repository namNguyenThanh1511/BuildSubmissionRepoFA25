using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAllAsync();
        Task<LeopardProfile> GetByIdAsync(int id);
        IQueryable<LeopardProfile> Search(string? modelName, double? weight);
        Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> AddAsync(LeopardProfile leopardProfile);
        Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> UpdateAsync(LeopardProfile leopardProfile);
        Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> DeleteAsync(int id);
    }
}
