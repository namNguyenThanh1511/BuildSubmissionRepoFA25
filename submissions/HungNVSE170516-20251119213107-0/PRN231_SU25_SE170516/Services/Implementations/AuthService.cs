using Microsoft.IdentityModel.Tokens;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.Models;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IAccountRepository accountRepository,JwtSettings jwtSettings)
        {
            _accountRepository = accountRepository;
            _jwtSettings = jwtSettings;
        }

        public string GenerateJwtToken(LeopardAccount account)
        {
           var role = GetRoleFromRoleId(account.RoleId);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.UserName),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetRoleFromRoleId(int? roleId)
        {
            if (roleId == null) return string.Empty;

            if (Enum.IsDefined(typeof(Roles), roleId.Value))
            {
                return ((Roles)roleId.Value).ToString();
            }

            return string.Empty;
        }

        public async Task<(bool Success, string Token, int Role)> LoginAsync(LoginRequest request)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(request.Email);

            if (account == null || account.Password != request.Password)
            {
                return (false, string.Empty, 0);
            }

            if (!IsValidRoleId(account.RoleId))
            {
                return (false, string.Empty, 0);
            }

            var token = GenerateJwtToken(account);
            return (true, token, account.RoleId);
        }
        private bool IsValidRoleId(int? roleId)
        {
            if (roleId == null) return false;
            return Enum.IsDefined(typeof(Roles), roleId.Value);
        }
    }
}
