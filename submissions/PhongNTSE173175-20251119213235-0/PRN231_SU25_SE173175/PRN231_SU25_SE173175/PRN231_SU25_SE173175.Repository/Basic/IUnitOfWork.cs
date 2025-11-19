namespace PRN231_SU25_SE173175.Repository.Basic
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<T> GetRepository<T>() where T : class;
		void Save();
		Task SaveAsync();
		void BeginTransaction();
		void CommitTransaction();
		void RollBack();
		Task<int> SaveChangesWithTransactionAsync();
	}
}

