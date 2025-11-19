using BLL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IProfileService
    {
        Task<IEnumerable<LeopardProfile>> GetAllItemsAsync();
        Task<LeopardProfile?> GetItemByIdAsync(int id);
        Task<LeopardProfile> AddItemAsync(ProfileRequestDto item);
        Task<LeopardProfile> UpdateItemAsync(int id, ProfileRequestDto item);
        Task DeleteItemAsync(int id);

    }
}
