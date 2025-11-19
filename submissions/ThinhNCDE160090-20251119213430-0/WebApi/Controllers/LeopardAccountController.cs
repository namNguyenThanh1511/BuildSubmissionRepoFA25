using BOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PE_PRN231_FA24_TrialTest_LeQuocUy_ODataAPI;
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardAccountController : ControllerBase
    {
        private readonly ILeopardAccountService _leopardAccountService;
        private readonly IConfiguration _configuration;  // <-- dùng để lấy secret
        public LeopardAccountController(ILeopardAccountService systemAccountsService, IConfiguration configuration)
        {
            _leopardAccountService = systemAccountsService;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeopardAccount>>> GetAll()
        {
            var accounts = await _leopardAccountService.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeopardAccount>> GetById(int id)
        {
            var account = await _leopardAccountService.GetByIdAsync(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }
        [Authorize(Roles = "5,6")]
        [HttpPost]
        public async Task<ActionResult<LeopardAccount>> Create(LeopardAccount account)
        {
            try
            {
                var created = await _leopardAccountService.CreateAsync(account);
                return CreatedAtAction(nameof(GetById), new { id = created.AccountId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _leopardAccountService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
        [AllowAnonymous]
        // LOGIN: tạo token trực tiếp trong controller
        [HttpPost("login")]
        public async Task<ActionResult<LeopardAccount>> Login([FromBody] LoginRequestDTO request)
        {
            var account = await _leopardAccountService.Login(request.Email, request.Password);

            if (account == null)
                return Unauthorized("Invalid email or password");

            // Tạo token trực tiếp
            var claims = new[]
            {
                new Claim("AccountId", account.AccountId.ToString()),
                new Claim("Role", account.RoleId?.ToString() ?? "0")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResponseDTO
            {
                Token = tokenString,
                Role = account.RoleId?.ToString(),
                AccountId = account.AccountId.ToString()
            });
        }
    }
}
