using BusinessLogic.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {

        private readonly ILeopardAccountService _authService;
        private readonly JWTHelper _jwtHelper;

        public authController(ILeopardAccountService authService, JWTHelper jwtHelper)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .FirstOrDefault();

                    return BadRequest(new
                    {
                        ErrorCode = "HB40001",
                        Message = firstError ?? "Invalid input"
                    });
                }

                var user = await _authService.LoginAsync(model.Email, model.Password);
                if (user == null)
                {
                    return Unauthorized(new
                    {
                        ErrorCode = "HB40101",
                        Message = "Invalid credentials"
                    });
                }

                string role;
                if (user.RoleId == 5)
                {
                    role = "administrator";
                }
                else if (user.RoleId == 6)
                {
                    role = "moderator";
                }
                else if (user.RoleId == 7)
                {
                    role = "developer";
                }
                else if (user.RoleId == 4)
                {
                    role = "member";
                }
                else
                {
                    return Unauthorized(new
                    {
                        ErrorCode = "HB40101",
                        Message = "Invalid credentials"
                    });
                }

                var token = _jwtHelper.GenerateJwtToken(user.Email, role);

                return Ok(new
                {
                    token = token,
                    role = user.RoleId,
                });
            }
            catch (Exception)
            {

                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }

        }
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
