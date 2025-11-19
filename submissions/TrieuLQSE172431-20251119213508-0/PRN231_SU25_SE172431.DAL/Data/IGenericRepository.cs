using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE172431.DAL.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }

        void Insert(T obj);

        void Update(T obj);

        void Delete(T Entity);

        void Save();
    }
}
