using Repositories.Interfaces;
using Repositories.Models;
using Services.Models.Requests.Auth;
using Services.Models.Responses.Auth;

namespace Services.Services
{
    public class AuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthServices(IUnitOfWork unitOfWork, JwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var users = await _unitOfWork.SystemAccounts
                .FindAsync(x => x.Email == request.Email && x.Password == request.Password);
            
            var user = users.FirstOrDefault();

            if (user == null)
                return null;

            // Check if user role is allowed to get a token
            var roleString = GetRoleString(user.RoleId);
            if (string.IsNullOrEmpty(roleString))
                return null;

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new LoginResponse
            {
                Token = token,
                Role = roleString
            };
        }

        private string? GetRoleString(int? roleId)
        {
            return roleId switch
            {
                UserRole.Administrator => "5",
                UserRole.Moderator => "6",
                UserRole.Developer => "7",
                UserRole.Member => "4",
                _ => null
            };
        }
    }
}
