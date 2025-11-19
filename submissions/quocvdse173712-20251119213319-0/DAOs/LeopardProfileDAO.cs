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
    public class LeopardProfileDAO
    {
        private SU25LeopardDBContext dbContext;
        private static LeopardProfileDAO instance;

        public LeopardProfileDAO()
        {
            dbContext = new SU25LeopardDBContext();
        }

        public static LeopardProfileDAO Instance
        {
            get
            {
                if (instance == null) instance = new LeopardProfileDAO();
                return instance;
            }
        }

        public async Task<List<LeopardProfile>> GetAllAsync() =>
            await dbContext.LeopardProfiles.ToListAsync();

        public async Task<LeopardProfile> GetByIdAsync(int id) =>
            await dbContext.LeopardProfiles.FirstOrDefaultAsync(h => h.LeopardProfileId == id);

        public async Task AddAsync(LeopardProfile t)
        {
            dbContext.LeopardProfiles.Add(t);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeopardProfile t)
        {
            dbContext.LeopardProfiles.Update(t);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeopardProfile t)
        {
            dbContext.LeopardProfiles.Remove(t);
            await dbContext.SaveChangesAsync();
        }

    }

}
