using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGenericRepository<T, Y> where T : class
    {
        // Synchronous methods
        List<T> GetAll(Func<IQueryable<T>, IQueryable<T>>? include = null);
        T GetById(Y id, Expression<Func<T, bool>> keySelector, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        T GetById(string code);
        T GetById(Guid code);
        void Create(T entity);
        void Update(T entity);
        bool Remove(T entity);

        // Asynchronous methods
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T> GetByIdAsync(Y id, Expression<Func<T, bool>> keySelector, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        Task<T> GetByIdAsync(string code);
        Task<T> GetByIdAsync(Guid code);
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<bool> RemoveAsync(T entity);

        // OData support
        IQueryable<T> GetQueryable();
    }
}
