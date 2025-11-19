using BusinessObjects;
using DataAccessObjects;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        public List<LeopardProfile> GetLeopardProfile()
        {
            return LeopardProfileDAO.Instance.GetLeopardProfile();
        }

        public LeopardProfile GetLeopardProfileById(int id)
        {
            return LeopardProfileDAO.Instance.GetLeopardProfileById(id);
        }

        public void AddLeopardProfile(LeopardProfileDTO item)
        {
            LeopardProfileDAO.Instance.AddLeopardProfile(item);
        }

        public void UpdateLeopardProfile(UpdateLeopardProfileDTO item)
        {
            LeopardProfileDAO.Instance.UpdateLeopardProfile(item);
        }

        public void DeleteLeopardProfile(int id)
        {
            LeopardProfileDAO.Instance.DeleteLeopardProfile(id);
        }
    }
}
