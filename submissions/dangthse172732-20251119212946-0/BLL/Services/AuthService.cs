using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _jwtSettings;
        public AuthService(IUnitOfWork unitOfWork, TokenService jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }

        public async Task<LoginResponse?> Login(LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return null;
            }

            var user = await _unitOfWork.GetRepository<LeopardAccount>().Entities
                .FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower() && x.Password == model.Password);
            
            if (user == null) return null;

            //string role = user.RoleId switch
            //{
            //    5 => "administrator",
            //    6 => "moderator",
            //    7 => "developer",
            //    4 => "member",
            //    _ => null
            //};

            //if (string.IsNullOrEmpty(user.RoleId.to)) return null;

            var token = _jwtSettings.GenerateToken(user.AccountId.ToString(), user.RoleId.ToString());

            return new LoginResponse
            {
                Role = user.RoleId.ToString(),
                //Role = role,
                Token = token
            };
        }
    }
}
