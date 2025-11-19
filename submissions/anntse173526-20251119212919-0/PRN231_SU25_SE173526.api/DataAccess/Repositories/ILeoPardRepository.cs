using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ILeoPardRepository
    {
        IEnumerable<LeopardProfile> GetAll();
        LeopardProfile? GetById(int id);
        void Add(LeopardProfile LeopardProfile);
        void Update(LeopardProfile LeopardProfile);
        void Delete(LeopardProfile LeopardProfile);
        IEnumerable<LeopardProfile> Search(string leopardName, double? weight);
        bool Exists(int id);
        void Save();
    }
}
