using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE173164.dal.Helpers;
using PRN231_SU25_SE173164.dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PRN231_SU25_SE173164.dal.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly Su25leopardDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(Su25leopardDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<T?> GetByIdAsync<TKey>(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> filter,
            string? includeProperties = null
        )
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (
                    var item in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null
        )
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (
                    var item in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Pagination<T>> GetPaginationAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeProperties = null,
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false
        )
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            IQueryable<T> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (
                    var item in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            var itemCount = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

    }
}
