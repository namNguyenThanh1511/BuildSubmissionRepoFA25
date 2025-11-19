using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILeoProfileService
    {
        Task<List<LeoResponse>> Get(int pageIndex, int pageSize);
        Task<List<LeoResponse>> Search(string? name, double? weight, int pageIndex, int pageSize);
        Task<LeoResponse> Get(int id);
        Task Create(LeoCreateModel model);
        Task Delete(int id);
        Task Update(int id, LeoCreateModel model);
    }
}
