using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface ILeopardProfileRepository
    {
        public IEnumerable<LeopardProfile> GetAll();
        public LeopardProfile GetById(int profileId);
        public LeopardProfile Create(LeopardProfile profile);
        public LeopardProfile Update(LeopardProfile profile);
        public void Delete(int profileId);
        IQueryable<LeopardProfile> Search(string leopardName);
    }
}
