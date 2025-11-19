using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE172431.DAL.Entities;

namespace PRN231_SU25_SE172431.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly Su25leopardDbContext _dbContext;
        private IGenericRepository<LeopardProfile>? _LeopardProfileRepository;
        private IGenericRepository<LeopardType>? _LeopardTypeRepository;
        private IGenericRepository<LeopardAccount>? _LeopardAccountRepository;

        public IGenericRepository<LeopardAccount> LeopardAccountRepository => _LeopardAccountRepository ??= new GenericRepository<LeopardAccount>(_dbContext);

        public IGenericRepository<LeopardProfile> LeopardProfileRepository => _LeopardProfileRepository ??= new GenericRepository<LeopardProfile>(_dbContext);
        public IGenericRepository<LeopardType> LeopardTypeRepository => _LeopardTypeRepository ??= new GenericRepository<LeopardType>(_dbContext);

        public UnitOfWork(Su25leopardDbContext dbContext)
        {
            _dbContext = dbContext;
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
                    Console.WriteLine("DbContext is being disposed.");
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
