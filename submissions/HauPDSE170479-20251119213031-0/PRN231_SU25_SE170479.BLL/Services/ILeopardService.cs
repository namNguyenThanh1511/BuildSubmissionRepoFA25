using PRN231_SU25_SE170479.DAL.DTOs;
using PRN231_SU25_SE170479.DAL.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE170479.BLL.Services
{
    public interface ILeopardService
    {
        Task<Result<List<GetLeopardProfileResponse>>> GetAllAsync();
        Task<Result<GetLeopardProfileResponse>> GetByIdAsync(int id);
        Task<Result<string>> CreateAsync(LeopardDTO dto);
        Task<Result<string>> UpdateAsync(int id, LeopardDTO dto);
        Task<Result<string>> DeleteByIdAsync(int id);
        Task<List<GetLeopardProfileResponse>> ListAllAsync();
    }
}
