using BLL.Interface;
using DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232_SU25_SE182972.ErrorModel;

namespace PRN232_SU25_SE182972.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var tokenresponse = await _authService.Login(loginRequestDTO);
            if (tokenresponse == null) return ErrorResponse.FromCode("HB40401");
            return new OkObjectResult(tokenresponse);
        }
    }
}
