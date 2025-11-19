using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILeopardTypeService
    {
        Task<List<LeopardType>> GetAll();
    }
    public class LeopardTypeService : ILeopardTypeService
    {
        private readonly LeopardTypeRepository _repository;

        public LeopardTypeService() => _repository = new LeopardTypeRepository();

        public async Task<List<LeopardType>> GetAll()
        {
            return await _repository.GetAll();
        }
    }
}
