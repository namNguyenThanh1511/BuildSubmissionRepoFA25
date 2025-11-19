using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ILeopardBL
    {

        Task<LeopardProfile> Create(LeopardProfile LeopardProfile);

        Task<List<LeopardProfile>> GetAll();

        Task<LeopardProfile> GetById(int id);

        Task<LeopardProfile> Update(LeopardProfile LeopardProfile);

        Task<LeopardProfile> Delete(int id);

        Task<List<LeopardProfile>> SearchAsync(string? leopardName);
    }
}
