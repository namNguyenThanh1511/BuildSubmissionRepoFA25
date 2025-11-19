using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BOs;

namespace STIService.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly Su25leopardDbContext _context;
        private Su25leopardDbContext context;

        public GenericRepository(Su25leopardDbContext context)
        {
            _context = context;
        }


        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = _context.Set<TEntity>();

            if (includeProperties.Any())
            {
                foreach (var property in includeProperties)
                {
                    items = items.Include(property);
                }
            }


            if (predicate != null)
            {
                items = items.Where(predicate);
            }

            return items;
        }


        public async Task<TEntity?> FindById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            // Thêm Include() với các thuộc tính liên quan
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }




        public void Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }


        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }


    }

}
