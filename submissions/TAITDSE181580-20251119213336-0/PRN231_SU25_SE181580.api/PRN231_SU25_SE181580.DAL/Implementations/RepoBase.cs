using PRN231_SU25_SE181580.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE181580.DAL.Entities;
using System.Linq.Expressions;

namespace PRN231_SU25_SE181580.DAL.Implementations {
    public class RepoBase<T>: IRepoBase<T> where T : class {
        protected readonly SU25LeopardDBContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepoBase(SU25LeopardDBContext context) {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity) {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity) {
            if (_context.Entry(entity).State == EntityState.Detached) {
                _dbSet.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
        }

        public Task DeleteAsync(T entity) {
            if (_context.Entry(entity).State == EntityState.Detached) {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<T> GetByIdAsync(object id) {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[] includeProperties) {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}

