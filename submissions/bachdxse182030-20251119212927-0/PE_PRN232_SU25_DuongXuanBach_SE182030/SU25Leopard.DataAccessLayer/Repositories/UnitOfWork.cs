using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SU25Leopard.BusinessObject.Models;
using SU25Leopard.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SU25Leopard.DataAccessLayer.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly Su25leopardDbContext _dbContext;
        private IDbContextTransaction? _transaction = null;
        
        private IGenericRepository<LeopardProfile>? _leopardProfileRepository;
        private IGenericRepository<LeopardType>? _leopardTypeRepository;
        private IGenericRepository<LeopardAccount>? _leopardAccountRepository;


        public UnitOfWork(
            Su25leopardDbContext dbContext,
            IGenericRepository<LeopardProfile> leopardProfileRepository,
            IGenericRepository<LeopardType> leopardTypeRepository,
            IGenericRepository<LeopardAccount> leopardAccountRepository
        )
        {
            _dbContext = dbContext;
            _leopardProfileRepository = leopardProfileRepository;
            _leopardTypeRepository = leopardTypeRepository;
            _leopardAccountRepository = leopardAccountRepository;
        }

        public IGenericRepository<LeopardProfile> LeopardProfileRepository => _leopardProfileRepository;
        public IGenericRepository<LeopardType> LeopardTypeRepository => _leopardTypeRepository;
        public IGenericRepository<LeopardAccount> LeopardAccountRepository => _leopardAccountRepository;
        // 🔹 Transaction - Dùng async để tránh block luồng
        //public async Task BeginTransactionAsync()
        //{
        //    _transaction = await _dbContext.Database.BeginTransactionAsync();
        //}

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                // Bắt đầu transaction
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();

                await operation(); // Gọi logic chính (thường là service gọi SaveChanges)

                await transaction.CommitAsync();
            });
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            });
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
