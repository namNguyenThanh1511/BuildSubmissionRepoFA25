using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE181580.BLL.DTOs;
using PRN231_SU25_SE181580.BLL.Interfaces;

namespace PRN231_SU25_SE181580.api.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController: ControllerBase {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService) {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request) {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Email)) {
                return StatusCode(400, new ErrorResponse("HB40001", "email is required"));
            }

            if (string.IsNullOrWhiteSpace(request.Password)) {
                return StatusCode(400, new ErrorResponse("HB40001", "password is required"));
            }

            try {
                var response = await _authenticateService.Login(request.Email, request.Password);
                return Ok(response);
            } catch (UnauthorizedAccessException) {
                return StatusCode(401, new ErrorResponse("HB40101", "Invalid email or password"));
            } catch (Exception) {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal server error"));
            }
        }
    }

    public class ErrorResponse {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string errorCode, string message) {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
