using Microsoft.AspNetCore.Mvc;
using PRN232_SU25_SE183867.service;
using PRN232_SU25_SE183867.service.Dto;

namespace PRN232_SU25_SE183867.api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginReq request)
        {
            try
            {
                return Ok(await _authService.Login(request));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
    }
}
