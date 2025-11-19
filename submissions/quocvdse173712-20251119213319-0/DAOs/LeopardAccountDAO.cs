using BusinessObjects.Models;
using DAOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class LeopardAccountDAO
    {
        private SU25LeopardDBContext dbContext;
        private static LeopardAccountDAO instance;

        public LeopardAccountDAO()
        {
            dbContext = new SU25LeopardDBContext();
        }

        public static LeopardAccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new LeopardAccountDAO();
                return instance;
            }
        }

        public async Task<LeopardAccount> LoginAsync(string email, string password)
        {
            return await dbContext.LeopardAccounts
                .FirstOrDefaultAsync(acc => acc.Email == email && acc.Password == password);
        }
    }
}
