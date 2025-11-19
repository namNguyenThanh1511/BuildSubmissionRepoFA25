using DAL.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IAccountRepository : IGenericRepository<LeopardAccount>
    {
        Task<LeopardAccount?> LoginAsync(string email, string password);
    }
}
