using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE172431.DAL.Entities;

namespace PRN231_SU25_SE172431.DAL.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();

        public void Dispose();
      
        IGenericRepository<LeopardAccount> LeopardAccountRepository { get; }

        IGenericRepository<LeopardProfile> LeopardProfileRepository { get; }

        IGenericRepository<LeopardType> LeopardTypeRepository { get; }
    }
}
