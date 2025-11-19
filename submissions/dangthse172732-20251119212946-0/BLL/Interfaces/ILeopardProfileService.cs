using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILeopardProfileService
    {
        Task<List<LeopartResponseModel>> Get();
        //Task<List<HandbagResponse>> Search(string? modelName, string? material);
        Task<LeopartResponseModel> Get(int id);
        Task Create(LeopardProfileCreateModel model);
        Task Delete(int id);
        Task Update(int id, LeopardProfileCreateModel model);
    }
}
