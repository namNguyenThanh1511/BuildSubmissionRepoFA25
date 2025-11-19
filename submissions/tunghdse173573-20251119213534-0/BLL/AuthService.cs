

using DAL;
using DAL.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace BLL
{
    public class AuthService
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
            var account = await _unitOfWork.GetRepository<LeopardAccount>().GetByPropertyAsync(x => x.Email == loginRequestDTO.email && x.Password == loginRequestDTO.password);

            if (account == null)
            {
                return null;
            }
            var role = Enum.GetName(typeof(RoleEnum), account.RoleId);
            LoginResponseDTO token = await Authentication.CreateToken(account, role, _jwtSettings);
            return token;
        }
    }
}
