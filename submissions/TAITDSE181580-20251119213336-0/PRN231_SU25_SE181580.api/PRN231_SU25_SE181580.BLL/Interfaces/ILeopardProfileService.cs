using PRN231_SU25_SE181580.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE181580.BLL.Interfaces {
    public interface ILeoPardProfileService {
        Task<IEnumerable<LeopardProfileDTO>> GetAllAsync();
        Task<LeopardProfileDTO?> GetByIdAsync(int id);
        Task<LeopardProfileDTO> CreateAsync(LeopardProfileDTO dto);
        Task<LeopardProfileDTO?> UpdateAsync(int id, LeopardProfileDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<IGrouping<string, LeopardProfileDTO>>> SearchAsync(string? modelName, double? material);
        IQueryable<LeopardProfileDTO> AsQueryable();
    }

}
