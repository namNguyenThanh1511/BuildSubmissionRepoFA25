using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repo;

        public LeopardProfileService(ILeopardProfileRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<LeopardProfile> GetAll() => _repo.GetAll();

        public LeopardProfile Get(int id) => _repo.GetById(id);
        public void Delete(int id) => _repo.Delete(id);
    }
}
