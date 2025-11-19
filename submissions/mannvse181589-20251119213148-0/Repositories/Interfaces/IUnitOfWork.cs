using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<LeopardProfile, int> Profiles { get; }
        IGenericRepository<LeopardType, int> Types { get; }
        IGenericRepository<LeopardAccount, int> SystemAccounts { get; }
        Task<int> SaveChangesAsync();
    }
}
