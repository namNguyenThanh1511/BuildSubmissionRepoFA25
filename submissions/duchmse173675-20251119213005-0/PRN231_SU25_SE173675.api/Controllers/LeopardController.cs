using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173675.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        private readonly ILeopardService _leopardService;

        public LeopardController(ILeopardService leopardService)
        {
            _leopardService = leopardService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Moderator, Developer, Member")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _leopardService.GetAllAsync();
            return Ok(profiles);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Administrator, Moderator, Developer, Member")]
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _leopardService.GetByIdAsync(id);
            if (profile == null)
                throw new KeyNotFoundException("Profile not found");
            return Ok(profile);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> CreateProfile([FromBody] LeopardRequest request)
        {
            if (request == null)
                throw new ApplicationException("Request body is missing");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProfile = await _leopardService.CreateAsync(request);
            return CreatedAtAction(nameof(GetProfileById), new { id = createdProfile.LeopardProfileId }, createdProfile);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] LeoparUpdatedRequest profileDto)
        {

            if (profileDto == null)
                throw new ApplicationException("Request body is missing");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _leopardService.UpdateAsync(id, profileDto);
            return Ok("Updated sucessfully");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            await _leopardService.DeleteAsync(id);
            return Ok("Deleted successfully");
        }

        [HttpGet("search")]
        [Authorize(Roles = "Administrator, Moderator, Developer, Member")]
        public async Task<IActionResult> Search([FromQuery] string? leopardName, [FromQuery] double? weight)
        {
            var result = await _leopardService.SearchAsync(leopardName, weight);

            if (result == null || !result.Any())
                throw new KeyNotFoundException("No profiles found with given criteria");

            return Ok(result);
        }
    }
}
