using AutoMapper;
using BusinessLogic.Service;
using DataAccess.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _leopardProfileService;
        private readonly IMapper _mapper;
        public LeopardProfileController(LeopardProfileService leopardProfileService, IMapper mapper)
        {
            _leopardProfileService = leopardProfileService;
            _mapper = mapper;
        }
        [Authorize(Policy = "all")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeopardProfileAsync()
        {
            var result = await _leopardProfileService.GetAllLeopardProfileAsync();
            return Ok(result);
        }
        [Authorize(Policy = "all")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            if (!int.TryParse(id, out int parsedId))
            {
                return ErrorHelper.BadRequest("Id must be integer");
            }

            var LeopardProfile = await _leopardProfileService.GetAllLeopardProfileAsync();
            var result = LeopardProfile.FirstOrDefault(c => c.LeopardProfileId == parsedId);
            if (result == null)
            {
                return ErrorHelper.NotFound("LeopardProfile don't exist");
            }
            return Ok(result);
        }
        [Authorize(Policy = "admin_or_mod")]
        [HttpPost]
        public async Task<IActionResult> CreateLeopardProfile([FromBody] LeopardProfileCreateDTO request)
        {
            var newLeopardProfile = new LeopardProfile
            {
                //LeopardProfileId = request.LeopardProfileId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                LeopardTypeId = request.LeopardTypeId,
                ModifiedDate = request.ModifiedDate
            };

            await _leopardProfileService.CreateLeopardProfileAsync(newLeopardProfile);

            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "admin_or_mod")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] LeopardProfileUpdateDTO request)
        {

            var existing = await _leopardProfileService.GetByIdAsync(id);
            if (existing != null)
            {
                existing.LeopardTypeId = request.LeopardTypeId;
                existing.LeopardName = request.LeopardName;
                existing.Weight = request.Weight;
                existing.Characteristics = request.Characteristics;
                existing.CareNeeds = request.CareNeeds;
                existing.ModifiedDate = request.ModifiedDate;
            }
            else return ErrorHelper.NotFound("LeopardProfile does not exist");

            var updated = await _leopardProfileService.UpdateAsync(existing);
            if (!updated)
                return ErrorHelper.InternalServerError("Failed to update LeopardProfile");

            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin_or_mod")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existing = await _leopardProfileService.GetByIdAsync(id);
            if (existing == null)
                return ErrorHelper.NotFound("LeopardProfile does not exist");

            var deleted = await _leopardProfileService.DeleteAsync(id);
            if (!deleted)
                return ErrorHelper.InternalServerError("Failed to delete LeopardProfile");

            return Ok();
        }
        [HttpGet("search")]
        [Authorize] // Any authenticated role
        public async Task<IActionResult> SearchAsync([FromQuery] string? modelName, [FromQuery] double? weight)
        {
            var results = await _leopardProfileService.SearchAsync(modelName, weight);

            var grouped = results
                .GroupBy(h => h.LeopardTypeName)
                .Select(g => new
                {
                    BrandName = g.Key,
                    LeopardProfiles = g.ToList()
                });

            return Ok(grouped);
        }
    }
}
