using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _service;

        public LeopardProfileController(LeopardProfileService service)
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
        public async Task<IActionResult> Search([FromQuery] string? leopardName)
        {
            var results = await _service.Search(leopardName);
            return Ok(results);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] CreateDTO dto)
        {
            //if (dto.LeopardProfileId <= 0)
            //    return BadRequest(new { errorCode = "HB40001", message = "LeopardProfileId is required and must be > 0" });

            var existing = await _service.GetById(dto.LeopardProfileId);
            if (existing != null)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardProfileId is exist" });

            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format" });

            if (dto.Weight == null || dto.Weight <= 15)
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15" });

            if (dto.LeopardTypeId == null || dto.LeopardTypeId <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardTypeId is required and must be > 0" });

            var lionPro = new LeopardProfile
            {
                LeopardProfileId = dto.LeopardProfileId,
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
            };

            var result = await _service.Create(lionPro);

            return CreatedAtAction(nameof(GetById), new { id = result }, dto);
        }


        [HttpPut]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update([FromBody] UpdateDTO dto)
        {
            if (dto.LeopardProfileId == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardProfileId format" });
            }

            var existing = await _service.GetById(dto.LeopardProfileId);
            if (existing == null)
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

            if (dto.LeopardName != null)
            {
                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (!regex.IsMatch(dto.LeopardName))
                    return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardName format" });

                existing.LeopardName = dto.LeopardName;
            }

            if (dto.Weight != null)
            {
                if (dto.Weight <= 0)
                    return BadRequest(new { errorCode = "HB40001", message = "Weight must be > 0" });
                existing.Weight = dto.Weight;
            }

            if (dto.Characteristics != null)
                existing.Characteristics = dto.Characteristics;

            if (dto.LeopardName != null)
                existing.LeopardName = dto.LeopardName;

                    if (dto.CareNeeds != null)
                existing.CareNeeds = dto.CareNeeds;



            if (dto.ModifiedDate != null)
                existing.ModifiedDate = (DateTime)dto.ModifiedDate;

            if (dto.LeopardTypeId != null)
                existing.LeopardType.LeopardTypeId = (int)dto.LeopardTypeId;

            await _service.Update(existing);

            var updated = await _service.GetById(dto.LeopardProfileId);
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
