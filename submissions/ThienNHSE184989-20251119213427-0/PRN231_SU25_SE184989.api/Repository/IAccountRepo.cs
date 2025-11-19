using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAccountRepo : IGenericRepository<LeopardAccount>
    {
        Task<LeopardAccount> Login(string email, string password);
    }
    public class AccountRepo : GenericRepository<LeopardAccount>, IAccountRepo
    {
        public AccountRepo(SU25LeopardDBContext context) : base(context)
        {
        }

        public Task<LeopardAccount> Login(string email, string password)
        {
            return _dbSet.FirstOrDefaultAsync(acc => acc.Email == email && acc.Password == password) ?? null;
        }
    }

}
