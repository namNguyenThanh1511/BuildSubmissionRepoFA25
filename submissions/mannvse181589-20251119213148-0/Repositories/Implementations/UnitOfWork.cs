using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Su25leopardDbContext _context;
        private bool _disposed;
        private IGenericRepository<LeopardAccount, int>? _systemAccounts;
        private IGenericRepository<LeopardProfile, int>? _profile;
        private IGenericRepository<LeopardType, int>? _type;
        public UnitOfWork(Su25leopardDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<LeopardProfile, int> Profiles => _profile ??= new GenericRepository<LeopardProfile,int>(_context);
        public IGenericRepository<LeopardType, int> Types => _type ??= new GenericRepository<LeopardType, int>(_context);

        public IGenericRepository<LeopardAccount, int> SystemAccounts => _systemAccounts ??= new GenericRepository<LeopardAccount, int>(_context);

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
