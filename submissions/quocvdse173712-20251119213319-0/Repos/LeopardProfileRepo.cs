using BusinessObjects.Models;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class LeopardProfileRepo : ILeopardProfileRepo
    {
        public Task<List<LeopardProfile>> GetAllAsync() => LeopardProfileDAO.Instance.GetAllAsync();
        public Task<LeopardProfile> GetByIdAsync(int id) => LeopardProfileDAO.Instance.GetByIdAsync(id);
        public Task AddAsync(LeopardProfile t) => LeopardProfileDAO.Instance.AddAsync(t);
        public Task UpdateAsync(LeopardProfile t) => LeopardProfileDAO.Instance.UpdateAsync(t);
        public Task DeleteAsync(LeopardProfile t) => LeopardProfileDAO.Instance.DeleteAsync(t);

    }
}
