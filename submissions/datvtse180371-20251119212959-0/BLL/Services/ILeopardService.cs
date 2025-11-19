using BLL.DTOs;

namespace BLL.Services;

public interface ILeopardService
{
    Task<IEnumerable<LeopardResponse>> GetAllLeopardsAsync();
    Task<LeopardResponse?> GetLeopardByIdAsync(int id);
    Task<LeopardResponse> CreateLeopardAsync(CreateLeopardRequest request);
    Task<LeopardResponse?> UpdateLeopardAsync(int id, UpdateLeopardRequest request);
    Task<bool> DeleteLeopardAsync(int id);
    Task<IEnumerable<IGrouping<string, LeopardResponse>>> SearchLeopardsAsync(string? leopardName, double? weight);
}