using BLL.DTOs;
using BLL.Interface;
using DAL.Enums;
using DAL.Interface;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _systemAccountRepo;
        private readonly JwtSettings _jwtSettings;
        public AccountService(IAccountRepository accountRepository, JwtSettings jwtSettings)
        {
            _systemAccountRepo = accountRepository;
            _jwtSettings = jwtSettings;
        }

        public string GenerateJwtToken(LeopardAccount account)
        {
            var role = GetRoleFromRoleId(account.RoleId);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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

        public async Task<(bool Success, string Token, string Role)> LoginAsync(LoginRequest request)
        {
            var account = await _systemAccountRepo.GetAccountByEmailAsync(request.Email);

            if (account == null || account.Password != request.Password)
            {
                return (false, string.Empty, string.Empty);
            }

            var role = GetRoleFromRoleId(account.RoleId);

            // Only allow specific roles to get tokens
            if (!IsValidRoleId(account.RoleId))
            {
                return (false, string.Empty, string.Empty);
            }

            var token = GenerateJwtToken(account);
            return (true, token, role);
        }
        private bool IsValidRoleId(int? roleId)
        {
            if (roleId == null) return false;
            return Enum.IsDefined(typeof(Roles), roleId.Value);
        }
    }
}
