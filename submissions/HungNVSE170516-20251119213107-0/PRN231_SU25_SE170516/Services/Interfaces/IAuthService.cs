using Repositories.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, int Role)> LoginAsync(LoginRequest request);
        string GenerateJwtToken(LeopardAccount account);
        //string GetRoleFromRoleId(int? roleId);
    }
}
