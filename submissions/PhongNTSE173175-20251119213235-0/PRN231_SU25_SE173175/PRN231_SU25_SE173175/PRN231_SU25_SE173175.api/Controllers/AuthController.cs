using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN231_SU25_SE173175.Repository.Base;
using PRN231_SU25_SE173175.Repository.Entities;
using PRN231_SU25_SE173175.Repository.Enums;
using PRN231_SU25_SE173175.Service.DTOs;
using PRN231_SU25_SE173175.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE173175.api.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _config;
		private readonly ILeopardAccountService _accountService;

		public AuthController(IConfiguration config, ILeopardAccountService accountService)
		{
			_config = config;
			_accountService = accountService;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
			{
				throw new BaseException(StatusCodes.Status400BadRequest, "Email and password are required");
			}

			var account = await _accountService.GetAccountByEmailAsync(request.Email);

			if (account == null || account.Password != request.Password)
			{
				throw new BaseException(StatusCodes.Status401Unauthorized, "Invalid email or password");
			}

			if (account.RoleId == null)
			{
				throw new BaseException(StatusCodes.Status403Forbidden, "Access denied");
			}

			var token = GenerateJSONWebToken(account);
			var response = new
			{
				Token = token,
				Role = account.RoleId
			};

			return Ok(response);
		}

		private string GenerateJSONWebToken(LeopardAccount systemUserAccount)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Jwt:Issuer"]
					, _config["Jwt:Audience"]
					, new Claim[]
					{
                //new(ClaimTypes.Name, systemUserAccount.Username),
                new(ClaimTypes.Email, systemUserAccount.Email),
				new(ClaimTypes.Role, systemUserAccount.RoleId.ToString()),
				new(ClaimTypes.NameIdentifier, systemUserAccount.AccountId.ToString()),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					},
					expires: DateTime.Now.AddMinutes(120),
					signingCredentials: credentials
				);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return tokenString;
		}
	}
}

