using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Helper;
using static PRN231_SU25_SE184012.api.ConstantsConfig;

namespace PRN231_SU25_SE184012.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AccountService AccountService,
        ITokenGenerator tokenGenerator) : ControllerBase
    {
        private readonly AccountService _systemAccountService =
            AccountService ?? throw new ArgumentNullException(nameof(AccountService));

        private readonly ITokenGenerator _tokenGenerator =
            tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestModels request)
        {
            var user = await _systemAccountService.GetSystemAccountAsync(
                request.Email,
                request.Password
            );
            if (user == null)
            {
                return BadRequest(
                    new ErrorModel(
                        ConstantsConfig.ErrorMessages.BadRequest,
                        ConstantsConfig.ErrorCodes.BadRequest
                    )
                );
            }

            var token = _tokenGenerator.GenerateToken(user);
            return Ok(
                new { Token = token, Role = ConstantsConfig.GetRoleName(user.RoleId.ToString()) }
            );
        }
    }
}
