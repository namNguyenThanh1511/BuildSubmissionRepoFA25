
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173175.Repository.Base;
using PRN231_SU25_SE173175.Repository.DBContext;
using System.Linq.Expressions;

namespace PRN231_SU25_SE173175.Repository.Basic
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly Su25leopardDbContext _context;
		protected readonly DbSet<T> _dbSet;
		public GenericRepository(Su25leopardDbContext dbContext)
		{
			_context = dbContext;
			_dbSet = _context.Set<T>();
		}
		public IQueryable<T> Entities => _context.Set<T>();

		public void Delete(object id)
		{
			T entity = _dbSet.Find(id) ?? throw new Exception();
			_dbSet.Remove(entity);
		}

		public async Task DeleteAsync(params object[] keyValues)
		{
			T entity = await _dbSet.FindAsync(keyValues) ?? throw new Exception("Entity not found.");
			_dbSet.Remove(entity);
		}

		public async Task DeleteAsync(object id)
		{
			T entity = await _dbSet.FindAsync(id) ?? throw new Exception();
			_dbSet.Remove(entity);
		}

		public IEnumerable<T> GetAll()
		{
			return _dbSet.AsEnumerable();
		}

		public async Task<IList<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public T? GetById(object id)
		{
			return _dbSet.Find(id);
		}

		public async Task<T?> GetByIdAsync(object id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<BasePaginatedList<T>> GetPagging(IQueryable<T> query, int? index, int? pageSize)
		{
			query = query.AsNoTracking();
			int count = await query.CountAsync();
			int pageIndex = (index > 0 ? index : 1) ?? 1;
			int pageSizeValue = (pageSize > 0 ? pageSize : 6) ?? 6;

			List<T> items = await query
				.Skip((pageIndex - 1) * pageSizeValue)
				.Take(pageSizeValue)
				.ToListAsync();
			return new BasePaginatedList<T>(items, count, index, pageSize);
		}

		public void Insert(T obj)
		{
			_dbSet.Add(obj);
		}

		public async Task InsertAsync(T obj)
		{
			await _dbSet.AddAsync(obj);
		}

		public void InsertRange(IList<T> obj)
		{
			_dbSet.AddRange(obj);
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public async Task SaveAsync()
		{
			var check = await _context.SaveChangesAsync();
			if (check == 0)
			{
				throw new Exception("Don't SaveChange!!!");
			}
		}

		public async Task<T?> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);
		public void Update(T obj)
		{
			_dbSet.Entry(obj).State = EntityState.Modified;
		}

		public Task UpdateAsync(T obj)
		{
			return Task.FromResult(_dbSet.Update(obj));
		}

		public async Task<IList<T>> SearchAsync(Expression<Func<T, bool>> filter)
		{
			return await _dbSet.Where(filter).ToListAsync();
		}

		public async Task<int> CountAsync()
		{
			return await _context.Set<T>().CountAsync();
		}

		public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> param)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(param);
		}

		public async Task<T?> FindByAndInclude(Expression<Func<T, bool>> param, Expression<Func<T, object>> include)
		{
			return await _context.Set<T>().Include(include).FirstOrDefaultAsync(param);
		}

		public async Task<T> FirstOrDefaultWithIncludesAsync(
		   Expression<Func<T, bool>> predicate,
		   Func<IQueryable<T>, IQueryable<T>>? includes = null
	   )
		{
			IQueryable<T> query = _dbSet;
			query = query.Where(predicate);

			if (includes != null)
			{
				query = includes(query);
			}

			return await query.FirstOrDefaultAsync();
		}
		public async Task<BasePaginatedList<T>> GetPaginationWithIncludesAsync(
			 string? keyword = null,
			Expression<Func<T, bool>>? predicate = null,
			Func<IQueryable<T>, IQueryable<T>>? includes = null,
			int pageIndex = 1,
			int pageSize = 10,
			Expression<Func<T, object>>? orderBy = null,
			bool isDescending = false
		)
		{
			IQueryable<T> query = _dbSet;

			if (!string.IsNullOrWhiteSpace(keyword))
			{
				var parameter = Expression.Parameter(typeof(T), "x");

				var properties = typeof(T).GetProperties()
					.Where(p => p.PropertyType == typeof(string));

				Expression? combined = null;

				foreach (var prop in properties)
				{
					var propertyAccess = Expression.Property(parameter, prop);
					var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));
					var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
					var keywordExpr = Expression.Constant(keyword);
					var containsExpr = Expression.Call(propertyAccess, containsMethod, keywordExpr);
					var finalExpr = Expression.AndAlso(nullCheck, containsExpr);

					combined = combined == null ? finalExpr : Expression.OrElse(combined, finalExpr);
				}

				if (combined != null)
				{
					var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
					query = query.Where(lambda);
				}
			}

			if (predicate != null)
				query = query.Where(predicate);

			if (includes != null)
				query = includes(query);

			var totalCount = await query.CountAsync();

			if (orderBy != null)
			{
				query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
			}

			var skip = (pageIndex - 1) * pageSize;
			var items = await query
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			return new BasePaginatedList<T>(items, totalCount, pageIndex, pageSize);
		}

	}
}

