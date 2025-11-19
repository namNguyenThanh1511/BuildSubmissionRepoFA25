using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class LeopardRepository : GenericRepository<LeopardProfile>, ILeopardRepository
{
    public LeopardRepository(Su25leopardDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LeopardProfile>> GetAllWithTypeAsync()
    {
        return await _dbSet.Include(h => h.LeopardType).ToListAsync();
    }

    public async Task<LeopardProfile?> GetByIdWithTypeAsync(int id)
    {
        return await _dbSet.Include(h => h.LeopardType).FirstOrDefaultAsync(h => h.LeopardTypeId == id);
    }

    public async Task<IEnumerable<LeopardProfile>> SearchAsync(string? leopardName, double? weight)
    {
        var query = _dbSet.Include(h => h.LeopardType).AsQueryable();

        if (!string.IsNullOrEmpty(leopardName))
        {
            query = query.Where(h => h.LeopardName.Contains(leopardName));
        }

        if (weight != null)
        {
            query = query.Where(h => h.Weight != null && h.Weight == weight);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<IGrouping<string, LeopardProfile>>> SearchGroupedByTypeAsync(string? leopardName, double? weight)
    {
        var handbags = await SearchAsync(leopardName, weight);
        return handbags.GroupBy(h => h.LeopardType?.LeopardTypeName ?? "Unknown Type");
    }
}