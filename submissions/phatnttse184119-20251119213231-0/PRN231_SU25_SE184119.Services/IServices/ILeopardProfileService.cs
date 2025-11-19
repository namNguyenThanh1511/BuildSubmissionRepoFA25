using PRN231_SU25_SE184119.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184119.Services.IServices
{
    public interface ILeopardProfileService
    {
        Task<IEnumerable<LeopardProfileDto>> GetAllAsync();
        Task<LeopardProfileDto?> GetByIdAsync(int id);
        Task<Dictionary<string, List<LeopardProfileDto>>> SearchAsync(string? modelName, string? material);
    }
}
