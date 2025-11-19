using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITypeService
    {
        Task<List<LeopardType>> GetAll();
    }
    public class LeopardTypeService : ITypeService
    {
        private readonly LeopardTypeRepo _repository;

        public LeopardTypeService() => _repository = new LeopardTypeRepo();

        public async Task<List<LeopardType>> GetAll()
        {
            return await _repository.GetAll();
        }
    }
}
