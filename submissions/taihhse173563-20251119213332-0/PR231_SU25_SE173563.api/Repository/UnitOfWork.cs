using Repository.Models;

namespace Repository
{
    public class UnitOfWork
    {
        private readonly Su25leopardDbContext _dbContext;
        public UnitOfWork(Su25leopardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
