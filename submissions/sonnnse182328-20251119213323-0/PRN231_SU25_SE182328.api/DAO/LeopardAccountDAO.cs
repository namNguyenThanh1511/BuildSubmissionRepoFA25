using BusinessObjects;

namespace DAO
{
    public class LeopardAccountDAO
    {
        private Su25leopardDbContext _dbContext;
        private static LeopardAccountDAO instance;

        public LeopardAccountDAO()
        {
            _dbContext = new Su25leopardDbContext();
        }

        public static LeopardAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardAccountDAO();
                }
                return instance;
            }
        }

        public LeopardAccount GetAccount(string email, string password)
        {
            return _dbContext.LeopardAccounts.SingleOrDefault(m => m.Email.Equals(email)
                                                         && m.Password.Equals(password));
        }
    }
}
