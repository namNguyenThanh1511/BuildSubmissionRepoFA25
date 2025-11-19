using PRN231_SU25_SE173171.BLL.DTOs;
using PRN231_SU25_SE173171.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.Interfaces
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>> GetAllList();
        Task<LeopardProfile> GetById(int id);
        Task Create(CreateLeopardProfileRequest request);
        Task Update(int id, UpdateLeopardProfileRequest request);
        Task Delete(int id);
    }
}
