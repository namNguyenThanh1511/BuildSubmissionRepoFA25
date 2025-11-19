using BLL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IAccountService
    {
        Task<(bool Success, string Token, string Role)> LoginAsync(LoginRequest request);
        string GenerateJwtToken(LeopardAccount account);
        string GetRoleFromRoleId(int? roleId);
    }
}
