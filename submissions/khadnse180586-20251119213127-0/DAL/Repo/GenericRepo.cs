using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
        public class GenericRepo<T> : IGenericRepo<T> where T : class
        {
            private readonly Su25leopardDbContext _context;
            private DbSet<T> _dbSet;
            public GenericRepo(Su25leopardDbContext context)
            {
                _context = context;
                _dbSet = _context.Set<T>();
            }

            protected DbSet<T> DbSet
            {
                get
                {
                    if (_dbSet != null)
                    {
                        return _dbSet;
                    }

                    _dbSet = _context.Set<T>();
                    return _dbSet;
                }
            }

            public async Task<IEnumerable<T>> GetAllAsync()
            {
                return await _dbSet.ToListAsync();
            }

            public async Task<T> GetByIdAsync(int id)
            {
                return await _dbSet.FindAsync(id);
            }

            public async Task AddAsync(T entity)
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(T entity)
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(T entity)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }

            public virtual async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }

            public virtual async Task<IEnumerable<T>> SearchAsync(
                Expression<Func<T, bool>> predicate,
                params Expression<Func<T, object>>[] includeProperties)
            {
                IQueryable<T> query = _dbSet.Where(predicate);

                // Apply include properties for eager loading
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

                return await query.ToListAsync();
            }
            public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
            {
                IQueryable<T> queryable = _dbSet;
                includeProperties = includeProperties?.Distinct().ToArray();
                if (includeProperties?.Any() ?? false)
                {
                    foreach (var navigationPath in includeProperties)
                    {
                        queryable = queryable.Include(navigationPath);
                    }
                }

                return predicate == null ? queryable : queryable.Where(predicate);
            }
            public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
            {
                return Get(predicate, includeProperties).FirstOrDefault();
            }
            public virtual async Task<IEnumerable<T>> SearchAsync(
                Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                params Expression<Func<T, object>>[] includeProperties)
            {
                IQueryable<T> query = _dbSet.Where(predicate);

                // Apply include properties for eager loading
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

                // Apply ordering if specified
                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                return await query.ToListAsync();
            }

            public async Task<int> GetMaxIdAsync(Expression<Func<T, int>> idSelector)
            {
                return await _context.Set<T>().AnyAsync()
                    ? await _context.Set<T>().MaxAsync(idSelector)
                    : 0;
            }

            public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            {
                return await _context.Set<T>().FirstOrDefaultAsync(predicate);
            }
        }
    }
