using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UOW
{
    public class UnitOfWork
    {
        private readonly Su25leopardDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(Su25leopardDbContext myStoreContext)
        {
            _dbContext = myStoreContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public GenericRepository<T> GetRepository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories[typeof(T)] = new GenericRepository<T>(_dbContext);
            }
            return (GenericRepository<T>)_repositories[typeof(T)];
        }
    }
}
