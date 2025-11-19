using Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ILeopardService
    {
        IEnumerable<LeopardResponse> GetAll();
        LeopardResponse? GetById(int id);
        string? Create(LeopardDto dto);
        string? Update(int id, LeopardDto dto);
        bool Delete(int id);
    }
}
