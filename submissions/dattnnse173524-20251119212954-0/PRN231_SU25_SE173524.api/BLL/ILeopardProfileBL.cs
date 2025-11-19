using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ILeopardProfileBL
    {
        Task<List<LeopardProfile>> GetAll();
        Task<LeopardProfile> GetById(int id);

        Task<LeopardProfile> Create(LeopardProfile leopard);

        Task<LeopardProfile> Update(int id, LeopardProfile leopard);

        Task<LeopardProfile> Delete(int id);

        Task<List<LeopardProfile>> SearchAsync(string? modelName, double weight);
    }
}
