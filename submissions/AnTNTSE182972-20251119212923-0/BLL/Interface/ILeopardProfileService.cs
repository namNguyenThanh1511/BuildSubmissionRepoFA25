using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfileViewDTO>> GetAllProfile();
        Task<LeopardProfileViewDTO> GetProfileById(int id);
        Task CreateNew (LeopardProfileModifyDTO model);
        Task UpdateProfile (LeopardProfileModifyDTO model, int id);
        Task DeleteProfile (int id);
        bool ValidateProduct(LeopardProfileModifyDTO leopardProfileModifyDTO);
    }
}
