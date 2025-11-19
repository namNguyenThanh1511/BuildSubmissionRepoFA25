using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.DTOs;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE173566.api.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAccountService accountService;

		public AuthController(IAccountService accountService)
		{
			this.accountService = accountService;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
		{
			var account = await accountService.Login(loginDTO.email, loginDTO.password);
			if (account == null)
			{
				return Unauthorized("Invalid email or password.");
			}

			//Generate JWT Token
			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true, true).Build();

			var claims = new List<Claim>
	  {
		  new Claim(ClaimTypes.Email, account.Email),
		  new Claim("Role", account.RoleId.ToString()),
		  new Claim("AccountId", account.AccountId.ToString()),
          ///Luu them thong tin khac neu can
      };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var preparedToken = new JwtSecurityToken(
				issuer: configuration["JWT:Issuer"],
				audience: configuration["JWT:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds);

			var Token = new JwtSecurityTokenHandler().WriteToken(preparedToken);
			var Role = account.RoleId; //0:Admin 1:Staff 2:Manager
			//var roleString = ConvertRoleToString(account.RoleId);
			var accountId = account.AccountId.ToString();
			return Ok(new LoginResponseDTO
			{
				role = Role,
				token = Token
			});
		}


	}
}
