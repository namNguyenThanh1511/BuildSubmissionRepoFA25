using BLL.DTOs;
using BLL.Responses;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE180740.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly AccountService _accountService;
        public authController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
            }
            try
            {
                var response = await _accountService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorResponse("HB40101", "Token missing/invalid"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("HB50001", "Error retrieving handbags"));
            }
        }

    }
}
