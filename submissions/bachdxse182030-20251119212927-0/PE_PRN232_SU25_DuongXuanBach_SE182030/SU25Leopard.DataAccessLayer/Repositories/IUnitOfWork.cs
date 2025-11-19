
using SU25Leopard.BusinessObject.Models;

namespace SU25Leopard.DataAccessLayer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
       IGenericRepository<LeopardProfile> LeopardProfileRepository { get; }
        IGenericRepository<LeopardType> LeopardTypeRepository { get; }
        IGenericRepository<LeopardAccount> LeopardAccountRepository { get; }


        //Task BeginTransactionAsync();
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
        Task ExecuteInTransactionAsync(Func<Task> operation);

        Task CommitTransactionAsync();
        void Dispose();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
        Task CommitAsync();

    }
}
