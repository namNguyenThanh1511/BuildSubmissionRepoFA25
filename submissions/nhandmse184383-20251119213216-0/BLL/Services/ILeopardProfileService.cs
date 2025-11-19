using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface ILeopardProfileService
    {
        Task<IEnumerable<LeopardProfileDto>> GetAllAsync();
        Task<LeopardProfileDto?> GetByIdAsync(int id);
        Task<(bool IsSuccess, ErrorResponseDto? Error)> CreateAsync(LeopardProfileCreateDto dto);
        Task<(bool IsSuccess, ErrorResponseDto? Error)> UpdateAsync(int id, LeopardProfileCreateDto dto);
        Task<(bool IsSuccess, ErrorResponseDto? Error)> DeleteAsync(int id);
        //Task<Dictionary<string, List<LeopardProfileDto>>> SearchAsync(string? modelName, string? material);
    }
}
