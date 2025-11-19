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
        private readonly LeopardTypeRepository _repository;
        public LeopardTypeService()
        {
            _repository = new LeopardTypeRepository();
        }
        public async Task<List<LeopardType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<LeopardType> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

    }
}
