using BLL.Interface;
using DAL;
using Data.DTO;
using Data.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly JWTSettings _jwtSettings;

        public AuthService(UnitOfWork unitOfWork, IOptions<JWTSettings> options)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = options.Value;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var account = await _unitOfWork.GetRepository<LeopardAccount>().GetByPropertyAsync(a => a.Email == loginRequestDTO.Email && a.Password == loginRequestDTO.Password);
            if (account == null) return null;
            LoginResponseDTO token = await Authentication.CreateToken(account!, account.RoleId!, _jwtSettings);
            return token;
        }

    }
}
