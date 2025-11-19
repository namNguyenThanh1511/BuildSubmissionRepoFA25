using System.Linq.Expressions;

namespace PRN231_SU25_SE181580.DAL.Interfaces {
    public interface IRepoBase<T> where T : class {
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}

