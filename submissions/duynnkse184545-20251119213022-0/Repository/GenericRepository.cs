using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GenericRepository<T, Y> : IGenericRepository<T, Y> where T : class
    {
        protected SU25LeopardDBContext _context;
        protected DbSet<T> _dbSet;

        public GenericRepository()
        {
            _context ??= new SU25LeopardDBContext();
            _dbSet = _context.Set<T>();
        }

        public GenericRepository(SU25LeopardDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public List<T> GetAll(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }

        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }
        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            try
            {
                _context.Add(entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during SaveChangesAsync: " + ex.Message);
                throw;
            }
        }


        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();

        }

        public async Task<int> UpdateAsync(T entity)
        {

            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
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

        public async Task<T> GetByIdAsync(
            Y id,
            Expression<Func<T, bool>> keySelector,
            Func<IQueryable<T>, IQueryable<T>> includeFunc = null)
        {
            IQueryable<T> query = _dbSet;

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            T entity = await query.FirstOrDefaultAsync(keySelector);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }


        public T GetById(
            Y id,
            Expression<Func<T, bool>> keySelector,
            Func<IQueryable<T>, IQueryable<T>> includeFunc = null)
        {
            IQueryable<T> query = _dbSet;

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            T entity = query.FirstOrDefault(keySelector);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public T GetById(string code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

        }

        public async Task<T> GetByIdAsync(string code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

        }

        public T GetById(Guid code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet;
        }

    }

}
