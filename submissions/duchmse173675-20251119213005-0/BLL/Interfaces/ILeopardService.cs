using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface ILeopardService
    {
        Task<IEnumerable<LeopardResponse>> GetAllAsync();
        Task<LeopardResponse> GetByIdAsync(int id);
        Task<LeopardResponse> CreateAsync(LeopardRequest request);
        Task UpdateAsync(int id, LeoparUpdatedRequest request);
        Task DeleteAsync(int id);
        Task<IEnumerable<LeopardResponse>> SearchAsync(string? leopardName, double? weight);
    }
}
