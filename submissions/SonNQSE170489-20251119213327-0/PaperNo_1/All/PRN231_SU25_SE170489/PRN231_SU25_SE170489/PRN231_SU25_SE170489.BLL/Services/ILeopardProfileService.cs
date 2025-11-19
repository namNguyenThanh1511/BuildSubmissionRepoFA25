using PRN231_SU25_SE170489.DAL.DTOs;
using PRN231_SU25_SE170489.DAL.ModelExtensions;

namespace PRN231_SU25_SE170489.BLL.Services
{
	public interface ILeopardProfileService
	{
		Task<Result<List<GetLeopardProfileResponse>>> GetAllAsync();
		Task<Result<GetLeopardProfileResponse>> GetByIdAsync(int id);
		Task<Result<string>> CreateAsync(LeopardProfileDTO dto);
		Task<Result<string>> UpdateAsync(int id, LeopardProfileDTO dto);
		Task<Result<string>> DeleteByIdAsync(int id);
		Task<List<GetLeopardProfileResponse>> ListAllAsync();
	}
}
