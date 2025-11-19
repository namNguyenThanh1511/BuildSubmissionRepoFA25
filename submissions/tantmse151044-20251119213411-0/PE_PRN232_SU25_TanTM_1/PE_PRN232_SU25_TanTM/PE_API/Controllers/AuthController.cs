using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs;
using Services.Interfaces;

namespace PE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly IAuthService _authService;

        public authController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng nhập với email và password
        /// </summary>
        /// <param name="loginRequest">Thông tin đăng nhập</param>
        /// <returns>JWT token và role</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return BadRequest(new { message = "Email và password không được để trống" });
                }

                var loginResponse = await _authService.LoginAsync(loginRequest);

                if (loginResponse == null)
                {
                    return Unauthorized(new { message = "Email hoặc password không đúng" });
                }

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        /// <summary>
        /// Kiểm tra token có hợp lệ không
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Kết quả validation</returns>
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token không được để trống" });
                }

                var isValid = await _authService.ValidateTokenAsync(token);

                if (isValid)
                {
                    return Ok(new { message = "Token hợp lệ" });
                }
                else
                {
                    return Unauthorized(new { message = "Token không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin role của user theo email
        /// </summary>
        /// <param name="email">Email của user</param>
        /// <returns>Role của user</returns>
        [HttpGet("role/{email}")]
        public async Task<IActionResult> GetUserRole(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { message = "Email không được để trống" });
                }

                var role = await _authService.GetUserRoleAsync(email);

                if (role != null)
                {
                    return Ok(new { email = email, role = role });
                }
                else
                {
                    return NotFound(new { message = "Không tìm thấy user với email này" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}