using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProfileService
    {
        Task<IEnumerable<ProfileModel>> GetAllAsync();
        Task<ProfileModel> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
