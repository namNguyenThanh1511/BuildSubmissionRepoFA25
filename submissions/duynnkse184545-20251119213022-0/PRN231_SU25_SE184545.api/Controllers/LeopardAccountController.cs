using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace PRN231_SU25_SE184545.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILeopardAccountService _leopardAccountService;
        private readonly IConfiguration _configuration;

        public AuthController(ILeopardAccountService leopardAccountService, IConfiguration configuration)
        {
            _leopardAccountService = leopardAccountService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] AccountRequestDTO loginDTO)
        {
            var result = await _leopardAccountService.Login(loginDTO, _configuration);
            return Ok(new { token = result.Token, role = result.Role });
        }

    }
}
