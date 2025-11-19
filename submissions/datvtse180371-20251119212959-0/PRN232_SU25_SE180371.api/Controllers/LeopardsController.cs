using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE180371.api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeopardsController : ControllerBase
{
    private readonly ILeopardService _leopardService;

    public LeopardsController(ILeopardService leopardService)
    {
        _leopardService = leopardService;
    }

    [HttpGet]
    [Authorize(Roles = "administrator,moderator,developer,member")]
    public async Task<IActionResult> GetAllLeopardProfiles()
    {
        try
        {
            var leopards = await _leopardService.GetAllLeopardsAsync();
            return Ok(leopards);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "administrator,moderator,developer,member")]
    public async Task<IActionResult> GetLeopardProfileById(int id)
    {
        try
        {
            var leopard = await _leopardService.GetLeopardByIdAsync(id);
            if (leopard == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "HB40401",
                    Message = "Resource not found"
                });
            }

            return Ok(leopard);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            });
        }
    }

    [HttpPost]
    [Authorize(Roles = "administrator,moderator")]
    public async Task<IActionResult> CreateLeopardProfile([FromBody] CreateLeopardRequest request)
    {
        if (!ModelState.IsValid)
        {
            var firstError = ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Missing/invalid input";

            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = firstError
            });
        }

        try
        {
            var leopard = await _leopardService.CreateLeopardAsync(request);
            return CreatedAtAction(nameof(GetLeopardProfileById), new { id = leopard.LeopardProfileId }, leopard);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "administrator,moderator")]
    public async Task<IActionResult> UpdateLeopardProfile(int id, [FromBody] UpdateLeopardRequest request)
    {
        if (!ModelState.IsValid)
        {
            var firstError = ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Missing/invalid input";

            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = firstError
            });
        }

        try
        {
            var leopard = await _leopardService.UpdateLeopardAsync(id, request);
            if (leopard == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "HB40401",
                    Message = "Resource not found"
                });
            }

            return Ok(leopard);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "administrator,moderator")]
    public async Task<IActionResult> DeleteLeopardProfile(int id)
    {
        try
        {
            var success = await _leopardService.DeleteLeopardAsync(id);
            if (!success)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "HB40401",
                    Message = "Resource not found"
                });
            }

            return Ok(new { message = "Leopard Profile deleted successfully" });
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            });
        }
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> SearchLeopardProfiles([FromQuery] string? leopardName, [FromQuery] double? weight)
    {
        try
        {
            var groupedProducts = await _leopardService.SearchLeopardsAsync(leopardName, weight);
            return Ok(groupedProducts);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "HB50001",
                Message = "Internal server error"
            });
        }
    }
}