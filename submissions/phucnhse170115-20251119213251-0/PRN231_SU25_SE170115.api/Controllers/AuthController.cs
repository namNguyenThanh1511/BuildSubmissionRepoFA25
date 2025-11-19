using Helper;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;
using System.Net;

namespace PRN231_SU25_SE170115.api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var user = await _authService.Login(model);
            if (user == null)
                return ErrorResultHelper.Create(HttpStatusCode.Unauthorized);

            var authResponse = await _authService.GenerateJwtToken(user.UserName, user.RoleId);

            return Ok(new
            {
                token = authResponse.Token,
                role = authResponse.Role
            });
        }
    }
}
