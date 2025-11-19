using Business.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class LeoPardService : ILeoPardService
    {
        private readonly ILeoPardRepository _repository;

        public LeoPardService(ILeoPardRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<LeopardProfile> GetAll() => _repository.GetAll();

        public LeopardProfile? GetById(int id) => _repository.GetById(id);

        public void Create(LeopardProfile LeopardProfile)
        {
            _repository.Add(LeopardProfile);
            _repository.Save();
        }

        public void Update(LeopardProfile LeopardProfile)
        {
            _repository.Update(LeopardProfile);
            _repository.Save();
        }

        public void Delete(int id)
        {
            var LeopardProfile = _repository.GetById(id);
            if (LeopardProfile != null)
            {
                _repository.Delete(LeopardProfile);
                _repository.Save();
            }
        }

        public IEnumerable<LeopardProfile> Search(string leopardName, double? weight)
        {
            return _repository.Search(leopardName, weight);
        }
    }
}
