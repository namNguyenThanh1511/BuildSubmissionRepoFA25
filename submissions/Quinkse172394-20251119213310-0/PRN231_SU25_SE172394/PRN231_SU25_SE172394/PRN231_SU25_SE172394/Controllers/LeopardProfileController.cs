using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE172394.DTOs;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE172394.Controllers
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

        [HttpGet("search")]
        [Authorize]
        [EnableQuery]
        public async Task<IActionResult> Search([FromQuery] string? leopardName, [FromQuery] float? weight)
        {
            var query = _service.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(leopardName))
            {
                query = query.Where(h => h.LeopardName.Contains(leopardName));
            }

            if (weight != null)
            {
                query = query.Where(h => h.Weight == weight);
            }

            return Ok(query);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] CreateDTO dto)
        {        
            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format" });

            if (dto.Weight == null || dto.Weight <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 0" });

            if (dto.Characteristics == null)
                return BadRequest(new { errorCode = "HB40001", message = "Characteristics can not be null" });

            if (dto.CareNeeds == null)
                return BadRequest(new { errorCode = "HB40001", message = "CareNeeds can not be null" });

            if (dto.LeopardTypeId == null || dto.LeopardTypeId <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardTypeId is required and must be > 0" });

            var leopardProfile = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
                LeopardTypeId = dto.LeopardTypeId
            };

            var result = await _service.Create(leopardProfile);

            return CreatedAtAction(nameof(GetById), new { id = result }, dto);
        }


		[HttpPut("{id}")]
		[Authorize(Roles = "administrator,moderator")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateDTO dto)
		{
			var existing = await _service.GetById(id);
			if (existing == null)
				return NotFound();

			if (dto.LeopardName != null)
			{
				var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
				if (!regex.IsMatch(dto.LeopardName))
					return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardName format" });

				existing.LeopardName = dto.LeopardName;
			}

			if (dto.Characteristics != null)
				existing.Characteristics = dto.Characteristics;

			if (dto.CareNeeds != null)
				existing.CareNeeds = dto.CareNeeds;
	
		    if (dto.Weight <= 0)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be > 0" });               
            }
            existing.Weight = dto.Weight;
            		
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
    }
}
