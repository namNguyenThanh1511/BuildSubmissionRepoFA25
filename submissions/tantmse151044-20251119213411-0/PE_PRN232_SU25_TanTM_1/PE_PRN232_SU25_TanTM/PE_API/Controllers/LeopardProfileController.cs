using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs;
using Services.Interfaces;

namespace PE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _profileService;

        public LeopardProfileController(ILeopardProfileService profileService)
        {
            _profileService = profileService;
        }

        /// <summary>
        /// Lists all LeopardProfile items
        /// </summary>
        /// <returns>List of all leopard profiles</returns>
        [HttpGet]
        [Authorize(Roles = "4,5,6,7")] // member, administrator, moderator, developer
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var profiles = await _profileService.GetAllProfilesAsync();
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves a specific LeopardProfile by its ID
        /// </summary>
        /// <param name="id">Profile ID</param>
        /// <returns>Leopard profile details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "4,5,6,7")] // member, administrator, moderator, developer
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var profile = await _profileService.GetProfileByIdAsync(id);

                if (profile == null)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        ErrorCode = "HB40401",
                        Message = "LeopardProfile not found"
                    });
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Creates a new LeopardProfile
        /// </summary>
        /// <param name="profileDto">Profile data</param>
        /// <returns>Created profile</returns>
        [HttpPost]
        [Authorize(Roles = "5,6")] // administrator, moderator only
        public async Task<IActionResult> Create([FromBody] LeopardProfileDTO profileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault();

                    return BadRequest(new ErrorResponseDTO
                    {
                        ErrorCode = "HB40001",
                        Message = errors ?? "Validation failed"
                    });
                }

                var createdProfile = await _profileService.CreateProfileAsync(profileDto);
                return CreatedAtAction(nameof(GetById), new { id = createdProfile.LeopardProfileId }, createdProfile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    ErrorCode = "HB40002",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Updates an existing LeopardProfile
        /// </summary>
        /// <param name="id">Profile ID</param>
        /// <param name="profileDto">Updated profile data</param>
        /// <returns>Updated profile</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "5,6")] // administrator, moderator only
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileDTO profileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault();

                    return BadRequest(new ErrorResponseDTO
                    {
                        ErrorCode = "HB40001",
                        Message = errors ?? "Validation failed"
                    });
                }

                var updatedProfile = await _profileService.UpdateProfileAsync(id, profileDto);
                return Ok(updatedProfile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ErrorResponseDTO
                {
                    ErrorCode = "HB40401",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Deletes a LeopardProfile
        /// </summary>
        /// <param name="id">Profile ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "5,6")] // administrator, moderator only
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _profileService.DeleteProfileAsync(id);

                if (!deleted)
                {
                    return NotFound(new ErrorResponseDTO
                    {
                        ErrorCode = "HB40401",
                        Message = "LeopardProfile not found"
                    });
                }

                return Ok(new { message = "LeopardProfile deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Search LeopardProfile by LeopardName and Weight
        /// </summary>
        /// <param name="leopardName">Leopard name to search</param>
        /// <param name="weight">Weight to search</param>
        /// <returns>Filtered profiles</returns>
        [HttpGet("search")]
        [Authorize(Roles = "4,5,6,7")] // member, administrator, moderator, developer
        public async Task<IActionResult> Search([FromQuery] string? leopardName, [FromQuery] double? weight)
        {
            try
            {
                var profiles = await _profileService.SearchProfilesAsync(leopardName, weight);
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error: " + ex.Message
                });
            }
        }
    }
}