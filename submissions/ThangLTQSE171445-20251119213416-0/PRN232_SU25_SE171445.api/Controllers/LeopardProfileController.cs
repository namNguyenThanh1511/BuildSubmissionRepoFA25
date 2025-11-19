using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using PRN232_SU25_SE171445.api.DTOs;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace PRN232_SU25_SE171445.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetById(id);
            if (item == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            return Ok(item);
        }

        [HttpGet("search-odata")]
        [Authorize]
        public async Task<ActionResult<List<GroupByDTO>>> Search(ODataQueryOptions<LeopardProfile> options)
        {
            IQueryable<LeopardProfile> query = _service.GetAllQueryable();

            var filteredQuery = options.ApplyTo(query);

            var profiles = await (filteredQuery as IQueryable<LeopardProfile>).ToListAsync();

            var groupedResults = profiles
                .GroupBy(h => h.LeopardType.LeopardTypeName)
                .Select(g => new GroupByDTO
                {
                    LeopardType = g.Key,
                    LeopardProfiles = g.OrderByDescending(x => x.LeopardName).ToList()
                })
                .ToList();

            return Ok(groupedResults);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] CreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format" });

            if (dto.Weight == null || dto.Weight <= 15)
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15" });

            if (dto.LeopardTypeId == null || dto.LeopardTypeId <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardTypeId is required and must be > 0" });

            if (dto.Characteristics == null)
                return BadRequest(new { errorCode = "HB40001", message = "Characteristics must be required" });

            if (dto.CareNeeds == null)
                return BadRequest(new { errorCode = "HB40001", message = "CareNeeds must be required" });

            if (dto.ModifiedDate == null)
                return BadRequest(new { errorCode = "HB40001", message = "ModifiedDate must be required" });

            //var newId = await _service.GetNextLeopardProfileId();


            var profile = new LeopardProfile
            {
                //LeopardProfileId = newId,
                LeopardTypeId = (int)dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = (double)dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = (DateTime)dto.ModifiedDate,
            };

            var result = await _service.Create(profile);

            if (profile.LeopardProfileId == 0)
                return StatusCode(500, new { errorCode = "HB50001", message = "Failed to create LionProfile" });

            var item = await _service.GetById(profile.LeopardProfileId);
            if (item == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            return CreatedAtAction(nameof(GetById), new { id = item.LeopardProfileId }, item);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDTO dto)
        {
            if (id == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Invalid HandbagId format" });
            }

            var existing = await _service.GetById(id);
            if (existing == null)
                return NotFound();

            if (dto.LeopardName != null)
            {
                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (!regex.IsMatch(dto.LeopardName))
                    return BadRequest(new { errorCode = "HB40001", message = "Invalid modelName format" });

                existing.LeopardName = dto.LeopardName;
            }

            if (dto.Characteristics != null)
                existing.Characteristics = dto.Characteristics;

            if (dto.CareNeeds != null)
                existing.CareNeeds = dto.CareNeeds;

            if (dto.Weight.HasValue)
            {
                if (dto.Weight <= 15)
                    return BadRequest(new { errorCode = "HB40001", message = "Weight must be > 15" });
                existing.Weight = (double)dto.Weight;
            }

            if (dto.ModifiedDate.HasValue)
                existing.ModifiedDate = (DateTime)dto.ModifiedDate;

            if (dto.LeopardTypeId.HasValue)
                existing.LeopardType.LeopardTypeId = (int)dto.LeopardTypeId;

            await _service.Update(existing);

            var updated = await _service.GetById(id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetById(id);
            if (existing == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            await _service.Delete(id);
            return Ok();
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<List<GroupByDTO>>> SearchByDb([FromQuery] string? leopardName, [FromQuery] string? weight)
        {
            var result = await _service.Search(leopardName, weight);
            return Ok(result);
        }
    }
}
