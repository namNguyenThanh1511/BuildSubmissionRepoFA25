using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE172431.BLL.Service;

namespace PRN231_SU25_SE172431.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly ILeoPardAccountService _systemAccountService;

        public authController(ILeoPardAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] BLL.DTO.Request.LoginRequest request)
        {
            var result = await _systemAccountService.Authentication(request);
            return Ok(result);

        }
    }
}
