using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service.DTO;

namespace Service
{
    public interface ILeopardAccountService
    {
        Task<AccountResponseDTO> Login(AccountRequestDTO loginDTO, IConfiguration configuration);
    }

    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _accountRepo;

        public LeopardAccountService(ILeopardAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<AccountResponseDTO> Login(AccountRequestDTO loginDTO, IConfiguration configuration)
        {

            var account = await _accountRepo.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Check if role is allowed to get token
            var roleNames = new Dictionary<int, string>
            {
                { 1, "administrator" },
                { 2, "moderator" },
                { 3, "developer" },
                { 4, "member" }
            };

            if (!roleNames.ContainsKey(account.RoleId))
            {
                throw new UnauthorizedAccessException("Role not authorized for token");
            }

            var roleName = roleNames[account.RoleId];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("Role", roleName),
                new Claim("AccountId", account.AccountId.ToString()),
            };

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var signCredential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signCredential);

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);

            return new AccountResponseDTO
            {
                Token = generatedToken,
                Role = roleName,
                AccountId = account.AccountId.ToString()
            };
        }
    }
}
