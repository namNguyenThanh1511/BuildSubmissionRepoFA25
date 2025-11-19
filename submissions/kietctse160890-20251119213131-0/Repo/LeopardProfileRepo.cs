using BO;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class LeopardProfileRepo : ILeopardProfile
    {
        public async Task<List<LeopardProfile>> GetLeopard()
        {
            return await LeopardProfileDAO.Instance.GetLeos();
        }
    }
}
