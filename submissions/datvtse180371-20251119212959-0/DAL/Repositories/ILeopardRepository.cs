using DAL.Models;

namespace DAL.Repositories;

public interface ILeopardRepository : IGenericRepository<LeopardProfile>
{
    Task<IEnumerable<LeopardProfile>> GetAllWithTypeAsync();
    Task<LeopardProfile?> GetByIdWithTypeAsync(int id);
    Task<IEnumerable<LeopardProfile>> SearchAsync(string? leopardName, double? weight);
    Task<IEnumerable<IGrouping<string, LeopardProfile>>> SearchGroupedByTypeAsync(string? leopardName, double? weight);
}