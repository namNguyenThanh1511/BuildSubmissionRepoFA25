using PRN232_SU25_SE170497.DAL.DTOs;
using PRN232_SU25_SE170497.DAL.ModelExtensions;

namespace BLL.Services
{
    public interface ILeopardProfileService
    {
        Task<Result<List<GetLeopardProfileRespone>>> GetAllAsync();
        Task<Result<GetLeopardProfileRespone>> GetByIdAsync(int id);
        Task<Result<string>> CreateAsync(LeopardProfileDTO dto);
        Task<Result<string>> UpdateAsync(int id, LeopardProfileDTO dto);
        Task<Result<string>> DeleteByIdAsync(int id);
        Task<List<GetLeopardProfileRespone>> ListAllAsync();
    }
}
