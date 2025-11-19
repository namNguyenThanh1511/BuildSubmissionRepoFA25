using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System.Linq.Expressions;

namespace Repository
{
    public class GenericRepository <T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly Su25leopardDbContext _dbcontext;
        public GenericRepository(Su25leopardDbContext dbontext)
        {
            _dbcontext = dbontext;
            _dbSet = dbontext.Set<T>();

        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await _dbSet.FindAsync(keyValues);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task<List<T>> GetAllByPropertyAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,string? thenInclude = null)
        {
            IQueryable<T> query = _dbSet;

            
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            if (thenInclude != null)
            {
                    query = query.Include(thenInclude.Trim());              
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<IQueryable<T>> GetQueryAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, string? thenInclude = null)
        {
            IQueryable<T> query = _dbSet;


            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            if (thenInclude != null)
            {
                query = query.Include(thenInclude.Trim());
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return  query;
        }
        public async Task<T> GetByPropertyAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            // query = query.AsNoTracking();
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
