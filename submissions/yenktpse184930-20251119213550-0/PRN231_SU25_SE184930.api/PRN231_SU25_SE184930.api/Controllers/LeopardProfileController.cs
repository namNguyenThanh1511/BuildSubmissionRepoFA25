using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE184930.bll.Interfaces;
using PRN231_SU25_SE184930.dal.DTOs;
using PRN231_SU25_SE184930.dal.Enums;
using System.Security.Claims;

namespace PRN231_SU25_SE184930.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _leopardService;

        public LeopardProfileController(ILeopardProfileService leopardService)
        {
            _leopardService = leopardService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Unauthorized,
                        Message = "Token missing/invalid"
                    });
                }

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole != "4" || userRole != "5" || userRole != "7" || userRole != "6")
                {
                    return StatusCode(403, new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Forbidden,
                        Message = "Permission denied"
                    });
                }

                var leopards = await _leopardService.GetAllAsync();
                return Ok(leopards);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.Unauthorized,
                    Message = "Token missing/invalid"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalServerError,
                    Message = "Internal server error"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Unauthorized,
                        Message = "Token missing/invalid"
                    });
                }

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(userRole) ||
                    !new[] { "administrator", "moderator", "developer", "member" }.Contains(userRole))
                {
                    return StatusCode(403, new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Forbidden,
                        Message = "Permission denied"
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.BadRequest,
                        Message = "Invalid Leopard ID"
                    });
                }

                var leopard = await _leopardService.GetByIdAsync(id);

                if (leopard == null)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.NotFound,
                        Message = "Leopard not found"
                    });
                }

                return Ok(leopard);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.Unauthorized,
                    Message = "Token missing/invalid"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalServerError,
                    Message = "Internal server error"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeopardProfileRequestDto request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Unauthorized,
                        Message = "Token missing/invalid"
                    });
                }

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(userRole) ||
                    !new[] { "administrator", "moderator" }.Contains(userRole))
                {
                    return StatusCode(403, new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.Forbidden,
                        Message = "Permission denied"
                    });
                }

                if (request == null)
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.BadRequest,
                        Message = "Request body is required"
                    });
                }

                if (string.IsNullOrEmpty(request.LeopardName))
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.BadRequest,
                        Message = "LeopardName is required"
                    });
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(request.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.BadRequest,
                        Message = "LeopardName format is invalid"
                    });
                }

                if (request.Weight <= 15)
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.BadRequest,
                        Message = "LeopardWeigt must be greater than 15"
                    });
                }

                if (request.LeopardTypeId <= 0)
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        ErrorCode = ErrorCodes.BadRequest,
                        Message = "LeopardType Id is required"
                    });
                }

                var leopard = await _leopardService.CreateAsync(request);
                return StatusCode(201, leopard);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.BadRequest,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.Unauthorized,
                    Message = "Token missing/invalid"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalServerError,
                    Message = "Internal server error"
                });
            }
        }
    }
}
