using System.Linq.Expressions;

namespace DAL.Repositories
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<List<T>> GetAllAsync();
        Task<int> CountAsync();
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<int> GetMaxIdAsync(Expression<Func<T, int>> idSelector);
    }

}
