using BLL.Interface;
using Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170197.api.Helper;

namespace PRN231_SU25_SE170197.api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var tokenmodel = await _authService.Login(loginRequestDTO);
            if (tokenmodel == null) return ErrorResponseHelper.FromCode("HB40401"); //Resource not found
            return new OkObjectResult(tokenmodel);
        }

    }
}
