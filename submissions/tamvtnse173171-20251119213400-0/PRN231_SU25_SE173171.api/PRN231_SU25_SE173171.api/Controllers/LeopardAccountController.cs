using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE173171.BLL.Base;
using PRN231_SU25_SE173171.BLL.DTOs;
using PRN231_SU25_SE173171.BLL.Interfaces;
using PRN231_SU25_SE173171.BLL.Store;

namespace PRN231_SU25_SE173171.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LeopardAccountController : ControllerBase
    {
        private readonly ILeopardAccountService _service;

        public LeopardAccountController(ILeopardAccountService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _service.Login(request);

                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(new BaseErrorResponse(ErrorCode.ErrorCodeMissingInvalidInput, ex.Message));
            }
        } 
    }
}
