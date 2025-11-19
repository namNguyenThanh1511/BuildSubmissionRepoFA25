using BEAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace BEAPI.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfilesController(ILeopardProfileService service)
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

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] string? LeopardName, [FromQuery] double? Weight)
        {
            var results = await _service.Search(LeopardName, Weight);
            return Ok(results);
        }

        [HttpGet("searchOData")]
        [Authorize]
        public async Task<ActionResult<List<LeopardProfile>>> Search(ODataQueryOptions<LeopardProfile> options)
        {
            IQueryable<LeopardProfile> query = _service.GetAllQueryable();

            var filteredQuery = options.ApplyTo(query);

            var leopardPros = await (filteredQuery as IQueryable<LeopardProfile>).ToListAsync();

            return Ok(leopardPros);
        }

        [HttpPost]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Create([FromBody] CreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format and required" });

            if (dto.Weight == null || dto.Weight <= 15)
                return BadRequest(new { errorCode = "HB40001", message = "Weight is required and must be greater than 15" });

            if (dto.LeopardTypeId == null || dto.LeopardTypeId <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardTypeId is required and must be > 0" });

            if (dto.Characteristics == null)
                return BadRequest(new { errorCode = "HB40001", message = "Characteristics is required" });

            if (dto.CareNeeds == null)
                return BadRequest(new { errorCode = "HB40001", message = "CareNeeds is required" });

            if (dto.ModifiedDate == null)
                return BadRequest(new { errorCode = "HB40001", message = "ModifiedDate is required" });

            var leopardPro = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Weight = (double)dto.Weight,
                CareNeeds = dto.CareNeeds,
                Characteristics = dto.Characteristics,
                ModifiedDate = (DateTime)dto.ModifiedDate,
                LeopardTypeId = (int)dto.LeopardTypeId
            };

            var result = await _service.Create(leopardPro);

            var newId = await _service.GetLeopardProfileId();

            var item = await _service.GetById(newId);
            if (item == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            return CreatedAtAction(nameof(GetById), new { id = item.LeopardProfileId }, item);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDto dto)
        {
            if (id == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardProfileId format" });
            }

            var existing = await _service.GetById(id);
            if (existing == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            if (dto.LeopardName != null)
            {
                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (!regex.IsMatch(dto.LeopardName))
                    return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardName format" });

                existing.LeopardName = dto.LeopardName;
            }

            if (dto.CareNeeds != null)
                existing.CareNeeds = dto.CareNeeds;

            if (dto.Characteristics != null)
                existing.Characteristics = dto.Characteristics;

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
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetById(id);
            if (existing == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            await _service.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }
    }
}
