using PRN231_SU25_SE181580.DAL.Interfaces;

namespace PRN231_SU25_SE181580.DAL.Interfaces {
    public interface IUnitOfWork: IDisposable {
        IRepoBase<T> GetRepo<T>() where T : class;
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackAsync();
        Task<int> SaveAsync();
    }
}

