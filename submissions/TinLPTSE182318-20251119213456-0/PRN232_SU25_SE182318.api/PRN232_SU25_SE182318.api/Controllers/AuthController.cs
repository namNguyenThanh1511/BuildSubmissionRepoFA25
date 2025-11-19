using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE182318.api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthController : Controller
    {
        private readonly ILeopardAccountService _service;
        
        public AuthController(ILeopardAccountService service)
        {
            _service = service;
        }

        [HttpPost("auth")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "Invalid email or password"
                });
            }

            var result = await _service.Login(dto);
            return Ok(result);
        }
    }
}
