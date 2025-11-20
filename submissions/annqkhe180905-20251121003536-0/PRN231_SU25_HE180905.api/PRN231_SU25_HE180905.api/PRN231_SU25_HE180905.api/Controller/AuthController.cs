using BusinessLogicLayer.Interface;
using DataAccessLayer.ReqAndRes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_HE180905.api.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "Missing/invalid input"
                });
            }

            var result = _accountService.Authenticate(request.Email, request.Password);
            if (result == null)
            {
                return Unauthorized(new
                {
                    errorCode = "HB40101",
                    message = "Token missing/invalid"
                });
            }

            return Ok(new
            {
                token = result.Token,
                role = result.Role
            });
        }
    }
}
