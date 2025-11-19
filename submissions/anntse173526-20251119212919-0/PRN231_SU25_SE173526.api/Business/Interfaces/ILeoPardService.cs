using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ILeoPardService
    {
        IEnumerable<LeopardProfile> GetAll();
        LeopardProfile? GetById(int id);
        void Create(LeopardProfile LeopardProfile);
        void Update(LeopardProfile LeopardProfile);
        void Delete(int id);
        IEnumerable<LeopardProfile> Search(string leopardName, double? weight);
    }
}
