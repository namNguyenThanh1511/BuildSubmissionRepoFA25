using DataAccess.Models;
using DataAccess.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ILeopardProfileService
    {

        Task<IEnumerable<LeopardProfile>> GetAllLeopardsAsync();
        Task<LeopardProfile?> GetLeopardByIdAsync(int id);
        Task<bool> CreateLeopardProfileAsync(LeopardProfileRequest leopardProfile);
        Task<bool> UpdateLeopardProfileAsync(int id, LeopardProfileRequest leopardProfile);
        Task<bool> DeleteLeopardProfileAsync(int id);
        Task<IEnumerable<Object>> SearchLeopardProfileAsync(string? LeopardName, double? Weight);
    }
}
