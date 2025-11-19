using PRN231_SU25_SE184930.dal.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.bll.Interfaces
{
    public interface ILeopardProfileService
    {
        Task<IEnumerable<LeopardProfileReponseDto>> GetAllAsync();
        Task<LeopardProfileReponseDto> GetByIdAsync(int id);
        Task<LeopardProfileReponseDto> CreateAsync(LeopardProfileRequestDto request);
    }
}
