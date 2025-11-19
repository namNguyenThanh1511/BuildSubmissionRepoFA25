using PRN231_SU25_SE173175.Repository.Base;
using System.Linq.Expressions;

namespace PRN231_SU25_SE173175.Repository.Basic
{
	public interface IGenericRepository<T> where T : class
	{
		// query
		IQueryable<T> Entities { get; }

		// non async
		IEnumerable<T> GetAll();
		T? GetById(object id);
		void Insert(T obj);
		void InsertRange(IList<T> obj);
		void Update(T obj);
		void Delete(object id);
		void Save();

		// async
		Task<IList<T>> GetAllAsync();
		Task<BasePaginatedList<T>> GetPagging(IQueryable<T> query, int? index, int? pageSize);
		Task<T?> GetByIdAsync(object id);
		Task InsertAsync(T obj);
		Task UpdateAsync(T obj);
		Task DeleteAsync(object id);
		Task DeleteAsync(params object[] keyValues);
		Task SaveAsync();
		Task<T?> FindAsync(params object[] keyValues);

		Task<int> CountAsync();
		Task<IList<T>> SearchAsync(Expression<Func<T, bool>> filter);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> param);
		Task<T?> FindByAndInclude(Expression<Func<T, bool>> param, Expression<Func<T, object>> include);

		Task<T> FirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? includes = null);

		Task<BasePaginatedList<T>> GetPaginationWithIncludesAsync(
			string? keyword = null,
			Expression<Func<T, bool>>? predicate = null,
			Func<IQueryable<T>, IQueryable<T>>? includes = null,
			int pageIndex = 1,
			int pageSize = 10,
			Expression<Func<T, object>>? orderBy = null,
			bool isDescending = false
		);
	}
}

