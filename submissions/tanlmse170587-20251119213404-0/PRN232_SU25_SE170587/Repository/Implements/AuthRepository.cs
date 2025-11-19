using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Requests;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly Su25leopardDbContext _context;

        public AuthRepository(Su25leopardDbContext context, IConfiguration configuration) 
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<MethodResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new MethodResult<LoginResponse>.Failure(new ErrorResponse("HB40001", "Email is required"), 400);
                }
                if (string.IsNullOrEmpty(request.Password))
                {
                    return new MethodResult<LoginResponse>.Failure(new ErrorResponse("HB40001", "Password is required"), 400);
                }

                var account = await _context.LeopardAccounts.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
                if (account == null)
                {
                    return new MethodResult<LoginResponse>.Failure(new ErrorResponse("HB40001", "Invalid Email"), 400);
                }
                if (account.Password != request.Password)
                {
                    return new MethodResult<LoginResponse>.Failure(new ErrorResponse("HB40001", "Wrong Password"), 400);
                }                
                
                var token = GenerateAccessToken(account);                

                return new MethodResult<LoginResponse>.Success(new LoginResponse(token, account.RoleId.ToString()), 200);                
            }
            catch (Exception e)
            {
                return new MethodResult<LoginResponse>.Failure(new ErrorResponse("HB50001", $"Internal server error: {e}"), 500);
            }
        }

        public string GenerateAccessToken(LeopardAccount account)
        {
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                new Claim("accountId", account.AccountId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            _ = int.TryParse(_configuration["JwtSettings:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: claimList,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
