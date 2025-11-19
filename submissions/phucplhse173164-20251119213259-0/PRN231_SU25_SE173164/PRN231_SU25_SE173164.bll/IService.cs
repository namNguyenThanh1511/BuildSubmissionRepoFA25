using PRN231_SU25_SE173164.bll.DTOs;
using PRN231_SU25_SE173164.dal.Entities;
using PRN231_SU25_SE173164.bll.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173164.bll
{
    public interface IService
    {
        Task<AuthenRespDTO> LoginAsync ( AuthenDTO authenDTO);
        Task<List<LeopardProfile>> GetAllLeopardProfile();
        Task<LeopardProfile> GetLeopardProfileByIdAsync(int id);
        Task CreateLeopardProfileAsync(LeopardProfileDTO leopard);
        Task UpdateLeopardProfileAsync(int id, LeopardProfileUpdateDTO leopard);
        Task DeleteLeopardProfileAsync(int id);
        IQueryable<LeopardProfile> GetSearch();
    }
}
