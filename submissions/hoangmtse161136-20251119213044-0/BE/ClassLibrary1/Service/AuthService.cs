using BusinessLogic.DTOs;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class AuthService : IAuthService
    {
        private readonly SU25LeopardDBContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(SU25LeopardDBContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            
            var user = await _context.LeopardAccounts
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);

            if (user == null)
                return null;

          
            if (!IsValidRole(user.RoleId))
                return null;

            string token = _jwtService.GenerateToken(user);
            string roleId = GetRoleId(user.RoleId);

            return new LoginResponse
            {
                Token = token,
                Role = roleId
            };
        }


        private bool IsValidRole(int? roleId)
        {
            return roleId is 5 or 6 or 7 or 4; 
        }

        private string GetRoleId(int? roleId)
        {
            return roleId switch
            {
                5 => "5",
                6 => "6",
                7 => "7",
                4 => "4",
                _ => "Unknown"
            };
        }
    }
}
