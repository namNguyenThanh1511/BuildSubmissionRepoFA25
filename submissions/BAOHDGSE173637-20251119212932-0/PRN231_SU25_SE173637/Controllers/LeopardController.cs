using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using PRN231_SU25_SE173637.DTO;
using Repositories.Interfaces;
using Repositories.Repository;

namespace PRN231_SU25_SE173637.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileRepository _profileRepo;

        public LeopardProfileController(ILeopardProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }

        [HttpGet]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _profileRepo.GetAllProfilesAsync();
            return Ok(profiles);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _profileRepo.GetProfileByIdAsync(id);
            if (profile == null)
                return NotFound(new { errorCode = "HB40401", message = "Profile not found" });
            return Ok(profile);
        }

        [HttpPost]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> CreateProfile([FromBody] LeopardProfileDto profileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errorCode = "HB40001", message = "Invalid input" });

            var profile = new LeopardProfile
            {
                LeopardTypeId = profileDto.LeopardTypeId,
                LeopardName = profileDto.LeopardName,
                Weight = profileDto.Weight,
                Characteristics = profileDto.Characteristics,
                CareNeeds = profileDto.CareNeeds,
                ModifiedDate = profileDto.ModifiedDate
            };

            await _profileRepo.CreateProfileAsync(profile);
            return Ok(profile);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] LeopardProfile profile)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errorCode = "HB40001", message = "Invalid input" });
            if (id != profile.LeopardProfileId)
                return BadRequest(new { errorCode = "HB40001", message = "Invalid input" });
            var existingProfile = await _profileRepo.GetProfileByIdAsync(id);
            if (existingProfile == null)
                return NotFound(new { errorCode = "HB40401", message = "Profile not found" });
            await _profileRepo.UpdateProfileAsync(profile);
            return Ok(profile);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _profileRepo.GetProfileByIdAsync(id);
            if (profile == null)
                return NotFound(new { errorCode = "HB40401", message = "Profile not found" });
            await _profileRepo.DeleteProfileAsync(id);
            return Ok();
        }

        [HttpGet("search")]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> SearchProfiles(string leopardName = null, string cheetahName = null, double? weight = null)
        {
            var profiles = await _profileRepo.SearchProfilesAsync(leopardName, cheetahName, weight);
            return Ok(profiles);
        }
    }
}