using BO;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class LeopardProfileDAO
    {
        private static LeopardProfileDAO instance = null;

        private LeopardProfileDAO()
        {
        }

        public static LeopardProfileDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardProfileDAO();
                }
                return instance;
            }
        }
        public async Task<List<LeopardProfile>> GetLeos()
        {
            using (var context = new Su25leopardDbContext())
            {
                var players = await context.LeopardProfiles.ToListAsync();
                return players;
            }
        }
    }
}
