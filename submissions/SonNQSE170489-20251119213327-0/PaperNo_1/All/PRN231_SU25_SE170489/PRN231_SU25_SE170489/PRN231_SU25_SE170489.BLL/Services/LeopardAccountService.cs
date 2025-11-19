using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE170489.DAL.ModelExtensions;
using PRN231_SU25_SE170489.DAL.Models;
using PRN231_SU25_SE170489.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE170489.BLL.Services
{
	public class LeopardAccountService : ILeopardAccountService
	{
		private readonly GenericRepository<LeopardAccount> _genericRepository;
		private readonly IConfiguration _config;

		public LeopardAccountService(GenericRepository<LeopardAccount> genericRepository, IConfiguration configuration)
		{
			_genericRepository = genericRepository;
			_config = configuration;
		}

		public async Task<Result<TokenResponse>> LoginAsync(string email, string password)
		{
			var user = (await _genericRepository.FindWithIncludeAsync(
				predicate: query => query.Email == email && query.Password == password)).FirstOrDefault();

			if (user == null)
			{
				return Errors.ValidationError<TokenResponse>("Invalid email or password");
			}

			var token = GenerateAccessToken(user);

			string roleName = user.RoleId switch
			{
				1 => "admin",
				2 => "moderator",
				3 => "developer",
				4 => "member",
				_ => "unknown"
			};

			var tokenResponse = new TokenResponse(Token: token, Role: roleName);

			return Result<TokenResponse>.Ok(tokenResponse);
		}

		private string GenerateAccessToken(LeopardAccount systemUserAccount)
		{
			try
			{
				var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
				var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(_config["JwtSettings:Issuer"]
						, _config["JwtSettings:Audience"]
						, new Claim[]
						{
			new(ClaimTypes.Name, systemUserAccount.UserName),
			new(ClaimTypes.Email, systemUserAccount.Email),
			new(ClaimTypes.Role, systemUserAccount.RoleId.ToString()),
						},
						expires: DateTime.Now.AddMinutes(120),
						signingCredentials: credentials
					);

				var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

				return tokenString;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}

	public record TokenResponse(string Token, string Role);
}
