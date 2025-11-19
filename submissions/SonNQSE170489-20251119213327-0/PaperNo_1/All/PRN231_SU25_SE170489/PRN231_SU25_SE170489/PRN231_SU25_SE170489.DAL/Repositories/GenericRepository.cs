using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE170489.DAL.DBContext;
using System.Linq.Expressions;

namespace PRN231_SU25_SE170489.DAL.Repositories
{
	public class GenericRepository<T> where T : class
	{
		protected SU25LeopardDBContext _context;

		public GenericRepository()
		{
			_context ??= new();
		}

		public GenericRepository(SU25LeopardDBContext context)
		{
			_context = context;
		}

		public IQueryable<T> GetIQueryable()
		{
			return _context.Set<T>();
		}

		public List<T> GetAll()
		{
			return _context.Set<T>().ToList();
		}
		public async Task<List<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}
		public void Create(T entity)
		{
			_ = _context.Add(entity);
			_ = _context.SaveChanges();
		}

		public async Task<int> CreateAsync(T entity)
		{
			_ = _context.Add(entity);
			return await _context.SaveChangesAsync();
		}
		public void Update(T entity)
		{
			//// Turning off Tracking for UpdateAsync in Entity Framework
			_context.ChangeTracker.Clear();
			var tracker = _context.Attach(entity);
			tracker.State = EntityState.Modified;
			_ = _context.SaveChanges();
		}

		public async Task<int> UpdateAsync(T entity)
		{
			//// Turning off Tracking for UpdateAsync in Entity Framework
			_context.ChangeTracker.Clear();
			var tracker = _context.Attach(entity);
			tracker.State = EntityState.Modified;
			return await _context.SaveChangesAsync();
		}

		public bool Remove(T entity)
		{
			_ = _context.Remove(entity);
			_ = _context.SaveChanges();
			return true;
		}

		public async Task<bool> RemoveAsync(T entity)
		{
			_ = _context.Remove(entity);
			_ = await _context.SaveChangesAsync();
			return true;
		}

		public T GetById(int id)
		{
			return _context.Set<T>().Find(id);
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public T GetById(string code)
		{
			return _context.Set<T>().Find(code);
		}

		public async Task<T> GetByIdAsync(string code)
		{
			return await _context.Set<T>().FindAsync(code);
		}


		public T GetById(Guid code)
		{
			return _context.Set<T>().Find(code);
		}

		public async Task<T> GetByIdAsync(Guid code)
		{
			return await _context.Set<T>().FindAsync(code);
		}

		public void PrepareCreate(T entity)
		{
			_ = _context.Add(entity);
		}

		public void PrepareUpdate(T entity)
		{
			var tracker = _context.Attach(entity);
			tracker.State = EntityState.Modified;
		}

		public void PrepareRemove(T entity)
		{
			_ = _context.Remove(entity);
		}

		public int Save()
		{
			return _context.SaveChanges();
		}

		public async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> FindWithIncludeAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? include = null, bool asNoTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();
			if (predicate != null)
				query = query.Where(predicate);
			if (asNoTracking)
				query = query.AsNoTracking();
			if (include != null)
				query = include(query);
			return await query.ToListAsync();
		}

		public async Task<int> GetMaxIntPropertyAsync(Expression<Func<T, int>> selector)
		{
			try
			{
				return await _context.Set<T>().MaxAsync(selector);
			}
			catch (InvalidOperationException)
			{
				return 0;
			}
		}
	}
}
