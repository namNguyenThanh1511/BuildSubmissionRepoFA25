using Repositories.DTOs;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ILeopardService
    {
        Task<IEnumerable<LeopardProfileDTO>> GetAllAsync();
        Task<LeopardProfileDTO> GetByIdAsync(int id);
        Task<(LeopardProfileDTO Created, string? Error)> CreateAsync(CreateLeopardDTO dto);
        Task<(LeopardProfile? Updated, string? Error)> UpdateAsync(int id, CreateLeopardDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
