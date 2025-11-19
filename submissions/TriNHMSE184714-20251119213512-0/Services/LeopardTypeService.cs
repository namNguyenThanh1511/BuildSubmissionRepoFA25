using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardTypeService
    {
        private readonly LeopardTypeRepository _repo;
        public LeopardTypeService() => _repo = new LeopardTypeRepository();

        public async Task<LeopardType?> GetById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }
}
