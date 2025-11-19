using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173635.api.Models;
using PRN231_SU25_SE173635.api.Models.DTOs;

namespace PRN231_SU25_SE173635.api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeopardProfileController : ControllerBase
{
    private readonly Su25leopardDbContext _context;

    public LeopardProfileController(Su25leopardDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "administrator,moderator,developer,member")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var profiles = await _context.LeopardProfiles
                .Include(p => p.LeopardType)
                .Select(p => new LeopardProfileResponse
                {
                    LeopardProfileId = p.LeopardProfileId,
                    LeopardTypeId = p.LeopardTypeId,
                    LeopardName = p.LeopardName,
                    Weight = p.Weight,
                    Characteristics = p.Characteristics,
                    CareNeeds = p.CareNeeds,
                    ModifiedDate = p.ModifiedDate,
                    LeopardTypeName = p.LeopardType.LeopardTypeName ?? string.Empty
                })
                .ToListAsync();

            return Ok(profiles);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "administrator,moderator,developer,member")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var profile = await _context.LeopardProfiles
                .Include(p => p.LeopardType)
                .FirstOrDefaultAsync(p => p.LeopardProfileId == id);

            if (profile == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ResourceNotFound,
                    Message = "LeopardProfile not found"
                });
            }

            var response = new LeopardProfileResponse
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardTypeId = profile.LeopardTypeId,
                LeopardName = profile.LeopardName,
                Weight = profile.Weight,
                Characteristics = profile.Characteristics,
                CareNeeds = profile.CareNeeds,
                ModifiedDate = profile.ModifiedDate,
                LeopardTypeName = profile.LeopardType.LeopardTypeName ?? string.Empty
            };

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }

    [HttpPost]
    [Authorize(Roles = "administrator,moderator")]
    public async Task<IActionResult> Create([FromBody] CreateLeopardProfileRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.MissingInput,
                    Message = "Invalid input data"
                });
            }

            var leopardType = await _context.LeopardTypes.FindAsync(request.LeopardTypeId);
            if (leopardType == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.MissingInput,
                    Message = "Invalid LeopardTypeId"
                });
            }

            var profile = new LeopardProfile
            {
                LeopardTypeId = request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                ModifiedDate = DateTime.Now
            };

            _context.LeopardProfiles.Add(profile);
            await _context.SaveChangesAsync();

            var response = new LeopardProfileResponse
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardTypeId = profile.LeopardTypeId,
                LeopardName = profile.LeopardName,
                Weight = profile.Weight,
                Characteristics = profile.Characteristics,
                CareNeeds = profile.CareNeeds,
                ModifiedDate = profile.ModifiedDate,
                LeopardTypeName = leopardType.LeopardTypeName ?? string.Empty
            };

            return CreatedAtAction(nameof(GetById), new { id = profile.LeopardProfileId }, response);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "administrator,moderator")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLeopardProfileRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.MissingInput,
                    Message = "Invalid input data"
                });
            }

            var profile = await _context.LeopardProfiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ResourceNotFound,
                    Message = "LeopardProfile not found"
                });
            }

            var leopardType = await _context.LeopardTypes.FindAsync(request.LeopardTypeId);
            if (leopardType == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.MissingInput,
                    Message = "Invalid LeopardTypeId"
                });
            }

            profile.LeopardTypeId = request.LeopardTypeId;
            profile.LeopardName = request.LeopardName;
            profile.Weight = request.Weight;
            profile.Characteristics = request.Characteristics;
            profile.CareNeeds = request.CareNeeds;
            profile.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            var response = new LeopardProfileResponse
            {
                LeopardProfileId = profile.LeopardProfileId,
                LeopardTypeId = profile.LeopardTypeId,
                LeopardName = profile.LeopardName,
                Weight = profile.Weight,
                Characteristics = profile.Characteristics,
                CareNeeds = profile.CareNeeds,
                ModifiedDate = profile.ModifiedDate,
                LeopardTypeName = leopardType.LeopardTypeName ?? string.Empty
            };

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "administrator,moderator")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var profile = await _context.LeopardProfiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ResourceNotFound,
                    Message = "LeopardProfile not found"
                });
            }

            _context.LeopardProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "LeopardProfile deleted successfully" });
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string? LeopardName, [FromQuery] double? Weight)
    {
        try
        {
            var query = _context.LeopardProfiles.Include(p => p.LeopardType).AsQueryable();

            if (!string.IsNullOrEmpty(LeopardName))
            {
                query = query.Where(p => p.LeopardName.Contains(LeopardName));
            }

            if (Weight.HasValue)
            {
                query = query.Where(p => p.Weight == Weight.Value);
            }

            var profiles = await query
                .Select(p => new LeopardProfileResponse
                {
                    LeopardProfileId = p.LeopardProfileId,
                    LeopardTypeId = p.LeopardTypeId,
                    LeopardName = p.LeopardName,
                    Weight = p.Weight,
                    Characteristics = p.Characteristics,
                    CareNeeds = p.CareNeeds,
                    ModifiedDate = p.ModifiedDate,
                    LeopardTypeName = p.LeopardType.LeopardTypeName ?? string.Empty
                })
                .ToListAsync();

            return Ok(profiles);
        }
        catch (Exception)
        {
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = ErrorCodes.InternalServerError,
                Message = "Internal server error"
            });
        }
    }
} 