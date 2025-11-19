using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;
        public GenericRepository(Su25leopardDbContext context)
        {
            DbSet = context.Set<TEntity>();
        }
        public void AddAsync(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void AddRangeAsync(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public void RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task<TEntity?> GetByIdWithIncludesAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, GetKeyPropertyName()) != null && EF.Property<TKey>(e, GetKeyPropertyName()).Equals(id));
        }

        public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return await query.ToListAsync();
        }

        private string GetKeyPropertyName()
        {
            var entityType = typeof(TEntity);
            return entityType.Name + "Id";
        }
    }
}
