using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface ILeopardProfileService
    {
        public IEnumerable<LeopardProfile> GetAllProfiles();
        public LeopardProfile GetProfileById(int profileId);
        public LeopardProfile CreateProfile(LeopardProfile profile);
        public LeopardProfile UpdateProfile(LeopardProfile profile);
        public void DeleteProfile(int profileId);
        public IQueryable<LeopardProfile> SearchProfile(string leopardName);
    }
}
