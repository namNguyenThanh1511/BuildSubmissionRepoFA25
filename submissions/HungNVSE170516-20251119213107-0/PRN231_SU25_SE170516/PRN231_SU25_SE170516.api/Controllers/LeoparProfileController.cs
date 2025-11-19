using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE170516.api.DTOs;
using Services.Interfaces;
using System.Security.Claims;

namespace PRN231_SU25_SE170516.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeoparProfileController : ControllerBase
    {
        private readonly IProfileService _service;

        public LeoparProfileController(IProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(role))
                {
                    return Unauthorized(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Token missing/invalid"
                    });
                }
                if (role != "admininstrator" && role != "moderator" && role != "developer" && role != "member")
                {
                    return StatusCode(403, new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Permission denied"
                    });
                }
                var profile = await _service.GetAllAsync();
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCodes.INTERNAL_ERROR,
                    Message = "Internal server error"
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(role))
                {
                    return Unauthorized(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Token missing/invalid"
                    });
                }
                if (role != "admininstrator" && role != "moderator" && role != "developer" && role != "member")
                {
                    return StatusCode(403, new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Permission denied"
                    });
                }

                var profile = await _service.GetByIdAsync(id);
                if (profile == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.NOT_FOUND,
                        Message = "Resource not found"
                    });
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCodes.INTERNAL_ERROR,
                    Message = "Internal server error"
                });
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(role))
                {
                    return Unauthorized(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Token missing/invalid"
                    });
                }
                if (role != "admininstrator" && role != "moderator")
                {
                    return StatusCode(403, new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Permission denied"
                    });
                }
                if (id < 0)
                {
                    return BadRequest(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_INPUT,
                        Message = "Invalid ID."
                    });
                }

                var success = await _service.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.NOT_FOUND,
                        Message = $"{id} not found."
                    });
                }

                return Ok(new { Message = "Deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = ErrorCodes.INTERNAL_ERROR,
                    Message = "Internal server error"
                });
            }
        }
    }
}
