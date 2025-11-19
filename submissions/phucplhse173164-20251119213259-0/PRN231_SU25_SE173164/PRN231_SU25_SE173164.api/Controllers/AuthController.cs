using PRN231_SU25_SE173164.bll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE173164.bll.DTOs;

namespace PRN231_SU25_SE173164.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IService _service;
        public AuthController(IService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenDTO model)
        {
            return Ok(await _service.LoginAsync(model));
        }
    }
}
