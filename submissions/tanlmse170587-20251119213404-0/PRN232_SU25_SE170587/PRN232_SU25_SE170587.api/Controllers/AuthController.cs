
using Microsoft.AspNetCore.Mvc;
using Repository.Implements;
using Repository.Interfaces;
using Repository.Requests;

namespace PRN232_SU25_SE170587.api.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authRepository.LoginAsync(request);
            return result.Match(
                (error, code) => StatusCode(code, error),     
                (data, code) => StatusCode(code, data)       
            );
        }
    }
}
