using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface ILeopardProfileService
    {
        IEnumerable<LeopardProfile> GetAll();
        LeopardProfile Get(int id);
        void Delete(int id);
    }
}
