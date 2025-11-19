using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LeopardProfileBL : ILeopardProfileBL
    {

        private readonly LeopardProfileDAO _dao;

        public LeopardProfileBL(LeopardProfileDAO dao)
        {
            _dao = dao;
        }
        public Task<LeopardProfile> Create(LeopardProfile leopard)
        {
            return _dao.AddAsync(leopard);
        }

        public Task<LeopardProfile> Delete(int id)
        {
            return _dao.DeleteAsync(id);
        }

        public Task<List<LeopardProfile>> GetAll()
        {
            return _dao.GetAllAsync();
        }

        public Task<LeopardProfile> GetById(int id)
        {
            return _dao.GetByIdAsync(id);
        }

        public Task<List<LeopardProfile>> SearchAsync(string? modelName, double weight)
        {
            return _dao.SearchAsync(modelName, weight);
        }

        public Task<LeopardProfile> Update(int id, LeopardProfile leopard)
        {
            return _dao.UpdateAsync(id, leopard);
        }
    }
}
