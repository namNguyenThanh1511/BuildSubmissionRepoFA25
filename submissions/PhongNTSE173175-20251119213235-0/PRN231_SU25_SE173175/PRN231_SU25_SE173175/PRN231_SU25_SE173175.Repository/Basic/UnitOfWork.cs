using PRN231_SU25_SE173175.Repository.DBContext;

namespace PRN231_SU25_SE173175.Repository.Basic
{
	public class UnitOfWork(Su25leopardDbContext dbContext) : IUnitOfWork
	{
		private bool disposed = false;
		private readonly Su25leopardDbContext _dbContext = dbContext;
		public void BeginTransaction()
		{
			_dbContext.Database.BeginTransaction();
		}

		public void CommitTransaction()
		{
			_dbContext.Database.CommitTransaction();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
			}
			disposed = true;
		}

		public void RollBack()
		{
			_dbContext.Database.RollbackTransaction();
		}

		public void Save()
		{
			_dbContext.SaveChanges();
		}

		public async Task SaveAsync()
		{
			await _dbContext.SaveChangesAsync();
		}

		public IGenericRepository<T> GetRepository<T>() where T : class
		{
			return new GenericRepository<T>(_dbContext);
		}

		public async Task<int> SaveChangesWithTransactionAsync()
		{
			using var transaction = await _dbContext.Database.BeginTransactionAsync();
			try
			{
				var result = await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return result;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				return -1;
			}
		}
	}
}

