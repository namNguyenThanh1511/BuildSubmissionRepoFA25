using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ILeopardService
    {
        Task<List<LeopardViewModel>> GetAllLeopard();
        Task<LeopardViewModel> GetLeopardById(int id);
        Task DeleteLeopard(int id);
    }
}
