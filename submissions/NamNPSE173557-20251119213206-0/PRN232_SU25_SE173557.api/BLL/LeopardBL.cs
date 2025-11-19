using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LeopardBL : ILeopardBL
    {
        private readonly LeopardDAO _LeopardDAO;
        public LeopardBL(LeopardDAO leopardDAO)
        {
            _LeopardDAO = leopardDAO;
        }

        public Task<LeopardProfile> Create(LeopardProfile leopardProfile)
        {
            return _LeopardDAO.AddAsync(leopardProfile);
        }


        public Task<List<LeopardProfile>> GetAll()
        {
            return _LeopardDAO.GetAllHsAsync();
        }

        public Task<LeopardProfile> GetById(int id)
        {
            return _LeopardDAO.GetByIdAsync(id);
        }


        public Task<LeopardProfile> Update(LeopardProfile leopardProfile)
        {
            return _LeopardDAO.UpdateAsync(leopardProfile);
        }


        Task<LeopardProfile> ILeopardBL.Delete(int id)
        {
            return _LeopardDAO.DeleteAsync(id);
        }


        public Task<List<LeopardProfile>> SearchAsync(string? leopardName)
        {
            return _LeopardDAO.SearchAsync(leopardName);
        }
    }
}
