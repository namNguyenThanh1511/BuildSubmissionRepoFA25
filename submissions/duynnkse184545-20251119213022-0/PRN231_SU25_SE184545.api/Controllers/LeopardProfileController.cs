using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace PRN231_SU25_SE184545.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _LeopardProfileService;

        public LeopardProfileController(ILeopardProfileService LeopardProfileService)
        {
            _LeopardProfileService = LeopardProfileService;
        }

        // GET /api/LeopardProfiles
        [HttpGet]
        [Authorize(Policy = "AllRoles")]
        public async Task<ActionResult> GetLeopardProfiles()
        {
            var result = await _LeopardProfileService.GetAll();
            return Ok(result);
        }

        // GET /api/LeopardProfiles/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AllRoles")]
        public async Task<ActionResult> GetLeopardProfile(int id)
        {
            var result = await _LeopardProfileService.GetById(id);
            return Ok(result);
        }

        // POST /api/LeopardProfiles
        [HttpPost]
        [Authorize(Policy = "AdminModerator")]
        public async Task<ActionResult> CreateLeopardProfile([FromBody] LeopardProfileRequest LeopardProfileDto)
        {
            var LeopardProfile = await _LeopardProfileService.CreateProfile(LeopardProfileDto);
            return StatusCode(201, LeopardProfile);
        }

        // PUT /api/LeopardProfiles/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminModerator")]
        public async Task<ActionResult> UpdateLeopardProfile(int id, [FromBody] LeopardProfileRequest LeopardProfileDto)
        {
            var updatedLeopardProfile = await _LeopardProfileService.UpdateProfile(id, LeopardProfileDto);
            return Ok(updatedLeopardProfile);
        }

        // DELETE /api/LeopardProfiles/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminModerator")]
        public async Task<ActionResult> DeleteLeopardProfile(int id)
        {
            await _LeopardProfileService.DeleteProfile(id);
            return Ok(new { message = "LeopardProfile deleted successfully" });
        }

        // GET /api/LeopardProfiles/search - Normal search with manual filtering
        [HttpGet("search")]
        [Authorize(Policy = "AllRoles")]
        public async Task<ActionResult> SearchLeopardProfiles([FromQuery] string? leopardName, [FromQuery] double? weight)
        {
            var result = await _LeopardProfileService.SearchProfiles(leopardName, weight);
            return Ok(result);
        }
    }

}
