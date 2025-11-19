using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ILeopardProfileRepository
    {
        IEnumerable<LeopardProfile> GetAll();
        LeopardProfile GetById(int id);
        void Delete(int id);
    }
}
