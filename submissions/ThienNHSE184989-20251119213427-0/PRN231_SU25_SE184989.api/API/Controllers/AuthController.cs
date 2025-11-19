using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Service;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IJWT _jwt;

        public AuthController(IAccountRepo accountRepo, IJWT jwt)
        {
            _accountRepo = accountRepo;
            _jwt = jwt;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] loginModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResult.InvalidInput());
            }

            var account = await _accountRepo.Login(request.Email, request.Password);
            if (account == null) return BadRequest(ErrorResult.InvalidInput());


            // Check if role is allowed (5=administrator, 6=moderator, 7=developer, 4=member)
            if (account.RoleId == null || account.RoleId < 4 || account.RoleId > 7)
                return BadRequest(ErrorResult.InvalidInput("HB40001", "Invalid role - no token issued"));

            int role = account.RoleId;
            string roleName = role switch
            {
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                4 => "member",
                _ => "unknown"
            };

            var token = _jwt.GenerateToken(account, role);

            return Ok(new
            {
                token = token,
                role = roleName
            });
        }
    }
    public class loginModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
