using DataAccess.Base;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LeopardProfileRepository : GenericRepository<LeopardProfile>
    {
        private readonly GenericRepository<LeopardProfile> genericRepository;

        public LeopardProfileRepository()
        {
            genericRepository = new GenericRepository<LeopardProfile>();

        }

        public async Task<List<LeopardProfile>> GellAllLeopardProfile()
        {
            return await genericRepository.GetAllAsync();
        }
        public Task<LeopardProfile> Getbyid(int id)
        {
            return genericRepository.GetByIdAsync(id);
        }

    }
}
