using BLL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173573.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var tokenmodel = await _authService.Login(loginRequestDTO);
            if (tokenmodel == null) return ErrorDefinitions.FromCode("HB40401");
            return new OkObjectResult(tokenmodel);
        }
    }
}
