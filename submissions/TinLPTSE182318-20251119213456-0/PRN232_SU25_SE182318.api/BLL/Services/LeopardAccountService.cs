using BLL.DTOs;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardAccountService : ILeopardAccountService 
    {
        private readonly IGenericRepo<LeopardAccount> _repo;
        private readonly IJwtService _jwtService;
        
        public LeopardAccountService(IGenericRepo<LeopardAccount> repo, IJwtService jwtService)
        {
            _repo = repo;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Login(LoginDTO dto)
        {
            string token = "";
            var account = await _repo.FirstOrDefaultAsync(la => la.Email == dto.Email && la.Password == dto.Password);
            if(account == null)
            {
                throw new CustomException("HB40101", "Invalid email or password", HttpStatusCode.Unauthorized);
            }

            var role = account.RoleId switch
            {
                4 => "member",
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                0 => ""
            };

            if (account.RoleId >= 4 && account.RoleId <= 7)
            {
                token = _jwtService.GenerateToken(account.Email, role);
            }

            return new LoginResponse
            {
                Token = token,
                Role = role,
            };
        }
    }
}
