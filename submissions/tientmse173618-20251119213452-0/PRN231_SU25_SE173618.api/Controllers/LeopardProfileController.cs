using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE173618.api.Models.DTOs;
using PRN231_SU25_SE173618.api.Services;
using System.Security.Claims;

namespace PRN231_SU25_SE173618.api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Jwt")]
public class LeopardProfileController : ControllerBase
{
    private readonly ILeopardProfileService _leopardProfileService;
    private readonly IAuthService _authService;

    public LeopardProfileController(ILeopardProfileService leopardProfileService, IAuthService authService)
    {
        _leopardProfileService = leopardProfileService;
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roleId = GetCurrentUserRoleId();
        if (!_authService.HasPermission(roleId, "read"))
        {
            return Forbid();
        }

        var profiles = await _leopardProfileService.GetAllAsync();
        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var roleId = GetCurrentUserRoleId();
        if (!_authService.HasPermission(roleId, "read"))
        {
            return Forbid();
        }

        var profile = await _leopardProfileService.GetByIdAsync(id);
        if (profile == null)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = "HB40401",
                Message = "LeopardProfile not found"
            });
        }

        return Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeopardProfileRequest request)
    {
        var roleId = GetCurrentUserRoleId();
        if (!_authService.HasPermission(roleId, "create"))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = "Invalid input data"
            });
        }

        try
        {
            var profile = await _leopardProfileService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = profile.LeopardProfileId }, profile);
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = "Failed to create LeopardProfile"
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLeopardProfileRequest request)
    {
        var roleId = GetCurrentUserRoleId();
        if (!_authService.HasPermission(roleId, "update"))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "HB40001",
                Message = "Invalid input data"
            });
        }

        var profile = await _leopardProfileService.UpdateAsync(id, request);
        if (profile == null)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = "HB40401",
                Message = "LeopardProfile not found"
            });
        }

        return Ok(profile);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var roleId = GetCurrentUserRoleId();
        if (!_authService.HasPermission(roleId, "delete"))
        {
            return Forbid();
        }

        var success = await _leopardProfileService.DeleteAsync(id);
        if (!success)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = "HB40401",
                Message = "LeopardProfile not found"
            });
        }

        return Ok(new { message = "LeopardProfile deleted successfully" });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] LeopardProfileSearchRequest request)
    {
        var roleId = GetCurrentUserRoleId();
        if (!_authService.HasPermission(roleId, "read"))
        {
            return Forbid();
        }

        var profiles = await _leopardProfileService.SearchAsync(request);
        return Ok(profiles);
    }

    private int GetCurrentUserRoleId()
    {
        var roleIdClaim = User.FindFirst("RoleId");
        if (roleIdClaim != null && int.TryParse(roleIdClaim.Value, out var roleId))
        {
            return roleId;
        }
        return 0;
    }
} 