using PRN232_SU25_SE181544.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE181544.DAL.Repositories
{
    public class LeopardAccountRepository
    {
        public async Task<LeopardAccount> Login(string email, string password)
        {
            using (Su25leopardDbContext context = new())
            {
                return context.LeopardAccounts.FirstOrDefault(x => x.Email == email && x.Password == password);
            }
        }
    }
}
