using BusinessObjects;
using DataAccessObjects;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeopardProfileRepository : ILeopardProfileRepository
    {
        public List<LeopardProfile> GetLeopardProfiles()
        {
            return LeopardProfileDAO.Instance.GetLeopardProfiles();
        }

        public LeopardProfile GetLeopardProfileById(int id)
        {
            return LeopardProfileDAO.Instance.GetLeopardProfileById(id);
        }

        public void AddLeopardProfile(LeopardProfileDTO item)
        {
            LeopardProfileDAO.Instance.AddLeopardProfile(item);
        }

        public void UpdateLeopardProfile(LeopardProfileDTO item)
        {
            LeopardProfileDAO.Instance.UpdateLeopardProfile(item);
        }

        public void DeleteLeopardProfile(int id)
        {
            LeopardProfileDAO.Instance.DeleteLeopardProfile(id);
        }
        public List<object> SearchLeopardProfiles(string? LeopardName, double? Weight)
        {
            return LeopardProfileDAO.Instance.SearchLeopardProfiles(LeopardName, Weight);
        }
    }
}
