using Microsoft.EntityFrameworkCore;
using Repo.Models;
using Repo.ModelsExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Base
{
    public class GenericRepository<T>
        where T : class
    {
        protected SU25LeopardDBContext _context;

        public GenericRepository()
        {
            _context ??= new SU25LeopardDBContext();
        }

        public GenericRepository(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        //insert create
        public async Task<T> InsertAsync(T entity)
        {
            var addedEntity = _context.Set<T>().Add(entity).Entity;
            await _context.SaveChangesAsync();
            return addedEntity;
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<T?> UpdateAsync(T entityToUpdate)
        {
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entityToUpdate;
        }

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T GetById(int? id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

            //return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

            //return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(string code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

            //return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(string code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

            //return await _context.Set<T>().FindAsync(code);
        }

        public T GetById(Guid code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

            //return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

            //return await _context.Set<T>().FindAsync(code);
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = ""
        )
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (
                    var includeProperty in includeProperties.Split(
                        new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(includeProperty.Trim());
                }
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<List<PaginationResult<T>>> GetAllPagedAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int pageIndex = 1,
            int pageSize = 10
        )
        {
            IQueryable<T> query = (
                await GetAllAsync(predicate, orderBy, includeProperties)
            ).AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new List<PaginationResult<T>>
            {
                new PaginationResult<T>
                {
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = pageIndex,
                    PageSize = pageSize,
                    Items = items,
                },
            };
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetQueryable(string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (
                    var includeProperty in includeProperties.Split(
                        new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query;
        }
    }
}
