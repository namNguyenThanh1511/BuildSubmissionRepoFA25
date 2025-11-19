using PRN231_SU25_SE173175.Repository.Entities;
using PRN231_SU25_SE173175.Service.DTOs;

namespace PRN231_SU25_SE173175.Service.Interfaces
{
	public interface ILeopardProfileService
	{
		Task<IEnumerable<LeopardProfileResponse>> GetAllAsync();
		Task<LeopardProfileResponse?> GetLeopardByIdAsync(int LeopardId);
		Task<LeopardProfileResponse> CreateLeopardAsync(LeopardProfileRequest Leopard);
		Task UpdateLeopardAsync(int id, LeopardProfileUpdateRequest Leopard);
		Task DeleteLeopardAsync(int id);
		Task<IQueryable<LeopardProfileResponse>> SearchLeopardsQueryableAsync(string? LeopardName, double? Weight);
	}
}
