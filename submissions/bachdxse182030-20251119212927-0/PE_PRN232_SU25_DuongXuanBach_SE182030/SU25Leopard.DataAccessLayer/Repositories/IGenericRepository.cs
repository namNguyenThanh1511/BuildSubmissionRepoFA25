using SU25Leopard.BusinessObject.Models;
using System.Linq.Expressions;

namespace SU25Leopard.DataAccessLayer.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetSingleByConditionAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Remove(Expression<Func<TEntity, bool>> predicate);
        Task<List<LeopardProfile>> search(string leopardName, double Weight);
    }
}
