using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE172431.DAL.Entities;

namespace PRN231_SU25_SE172431.DAL.Data
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly Su25leopardDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(Su25leopardDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> Entities => _dbSet;

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;
        }

    }
}
