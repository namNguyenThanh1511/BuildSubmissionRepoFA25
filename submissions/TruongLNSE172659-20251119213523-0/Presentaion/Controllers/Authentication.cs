using Bisiness.Iservice;
using DataAccess.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Presentaion.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class Authentication: ControllerBase
    {
        public readonly ILeopardAccountService _leopardAccountService;
        public Authentication(ILeopardAccountService leopardAccountService)
        {
            _leopardAccountService = leopardAccountService ?? throw new ArgumentNullException(nameof(leopardAccountService));
        }

        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginModelRequest request)
        {
            var a = await _leopardAccountService.login(request);
            return Ok(a);
        }
    }
}
