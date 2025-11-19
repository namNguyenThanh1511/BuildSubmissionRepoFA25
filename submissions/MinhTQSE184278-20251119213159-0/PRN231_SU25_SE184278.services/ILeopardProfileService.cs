using PRN231_SU25_SE184278.repositories;
using PRN231_SU25_SE184278.services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.services
{
    public interface ILeopardProfileService
    {
        Task<IEnumerable<LeopardProfileDto>> GetAllAsync();

        Task<LeopardProfileDto?> GetByIdAsync(int id);

        Task<(bool IsSuccess, ErrorResponseDto? Error)> CreateAsync(LeopardProfileCreateDto dto);


        Task<(bool IsSuccess, ErrorResponseDto? Error)> UpdateAsync(int id, LeopardProfileCreateDto dto);

        Task<(bool IsSuccess, ErrorResponseDto? Error)> DeleteAsync(int id);


        Task<List<LeopardProfileDto>> SearchAsync(string? LeopardName, double? Weight);

    }
}
