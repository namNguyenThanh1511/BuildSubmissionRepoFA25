using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<TEntity, in TKey> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdWithIncludesAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
        void AddRangeAsync(IEnumerable<TEntity> entities);
        void RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
