using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UOW
{
    public class GenericRepository<T> where T : class
    {
        protected readonly Su25leopardDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(Su25leopardDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> Entities => _context.Set<T>();

        public void Delete(T obj)
        {
            _dbSet.Remove(obj);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;
        }
    }
}
