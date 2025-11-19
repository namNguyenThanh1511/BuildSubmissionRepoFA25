using BLL;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173573.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _leopardProfileService;
        public LeopardProfileController(LeopardProfileService leopardProfileService)
        {
            _leopardProfileService = leopardProfileService;
        }
        [Authorize(Roles = "administrator,moderator,member,developer")]
        [HttpGet("GetAllLeopardProfile")]
        public async Task<IActionResult> GetAllLeopardProfiles()
        {
            var leopardProfiles = await _leopardProfileService.GetAllLeopards();
            return new OkObjectResult(leopardProfiles);
        }
        [Authorize(Roles = "administrator,moderator,member,developer")]
        [HttpGet("GetLeopardProfileById")]
        public async Task<IActionResult> GetLeopardProfileById(int id)
        {
            var leopard = await _leopardProfileService.GetLeopardProfile(id);
            if (leopard == null) return ErrorDefinitions.FromCode("HB40401");
            return new OkObjectResult(leopard);
        }
        [HttpGet("SearchLeopardProfiles")]
        public async Task<IActionResult> SearchLeopardProfiles(string? cheetarName, double? weight)
        {
            var LeopardProfile = await _leopardProfileService.SearchLeopardProfile(cheetarName, weight);
            if (LeopardProfile == null) return ErrorDefinitions.FromCode("HB40401");
            return new OkObjectResult(LeopardProfile);
        }
        [Authorize(Roles = "administrator,moderator")]
        [HttpPost("CreateLeopardProfile")]
        public async Task<IActionResult> CreateLeopardProfile([FromBody] LeopardProfileDTo leopardProfileDTo)
        {
            if (!ModelState.IsValid)
            {
                return ErrorDefinitions.FromCode("HB40001");
            }
            var checkvalidate = _leopardProfileService.ValidateProduct(leopardProfileDTo);

            if (checkvalidate == false) return ErrorDefinitions.FromCode("HB40001");

            try
            {
                await _leopardProfileService.CreateNewLeopardProfile(leopardProfileDTo);
                return new OkObjectResult("Create successfully");
            }
            catch
            {
                return ErrorDefinitions.FromCode("HB50001");
            }
        }
        [Authorize(Roles = "administrator,moderator")]
        [HttpPut("UpdateLeopardProfile")]
        public async Task<IActionResult> UpdateLeopardProfile([FromQuery] int id, [FromBody] LeopardProfileDTo leopardProfileDTo)
        {
            if (!ModelState.IsValid)
            {
                return ErrorDefinitions.FromCode("HB40001");
            }
            var LeopardProfile = await _leopardProfileService.GetLeopardProfile(id);
            if (LeopardProfile == null) return ErrorDefinitions.FromCode("HB40401");
            var checkvalidate = _leopardProfileService.ValidateProduct(leopardProfileDTo);
            if (checkvalidate == false) return ErrorDefinitions.FromCode("HB40001");
            try
            {
                await _leopardProfileService.UpdateLeopardProfile(id, leopardProfileDTo);
                return new OkObjectResult("Update successfully");

            }
            catch
            {
                return ErrorDefinitions.FromCode("HB50001");
            }
        }
        [Authorize(Roles = "administrator,moderator")]
        [HttpDelete("DeleteleopardProfile")]
        public async Task<IActionResult> DeleteLeopardProfile([FromQuery] int id)
        {
            var leopard = await _leopardProfileService.GetLeopardProfile(id);
            if (leopard == null) return ErrorDefinitions.FromCode("HB40401");
            try
            {
                await _leopardProfileService.DeleteLeopardProfile(id);
                return new OkObjectResult("Delete successfully");

            }
            catch
            {
                return ErrorDefinitions.FromCode("HB50001");
            }
        }

    }
}
