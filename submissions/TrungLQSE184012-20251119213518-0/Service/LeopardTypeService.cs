using Repo.Base;
using Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LeopardTypeService(GenericRepository<LeopardType> repo)
    {
        private readonly GenericRepository<LeopardType> _repo = repo;

        public async Task<LeopardType?> GetLeopardTypeAsync(int id)
        {
            if (id <= 0)
                return null;
            return await _repo.GetOneAsync(x => x.LeopardTypeId == id);
        }

        public async Task<List<LeopardType>> GetAllLeopardTypesAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
