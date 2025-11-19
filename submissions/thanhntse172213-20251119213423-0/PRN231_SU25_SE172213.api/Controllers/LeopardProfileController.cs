using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE172213.api.DTOs;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE172213.api.Controllers
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
        public async Task<IActionResult> Search([FromQuery] string? leopardName, [FromQuery] double? weight)
        {
            var results = await _service.Search(leopardName, weight);
            return Ok(results);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] CreateDTO dto)
        {
            // Thêm validation cho các trường bắt buộc và định dạng để ra đúng error format

            if (dto.LeopardProfileId == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "LeopardProfileId is required" });
            }

            if (dto.LeopardTypeId == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "LeopardTypeId is required" });
            }

            if (string.IsNullOrWhiteSpace(dto.LeopardName))
            {
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is required" });
            }

            if (dto.Weight == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Weight is required" });
            }

            if (string.IsNullOrWhiteSpace(dto.Characteristics))
            {
                return BadRequest(new { errorCode = "HB40001", message = "Characteristics is required" });
            }

            if (string.IsNullOrWhiteSpace(dto.CareNeeds))
            {
                return BadRequest(new { errorCode = "HB40001", message = "CareNeeds is required" });
            }

            if (dto.ModifiedDate == null)
            {
                return BadRequest(new { errorCode = "HB40001", message = "ModifiedDate is required" });
            }

            if (dto.LeopardProfileId <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardProfileId must be > 0" });
             
            int leoId = (int)dto.LeopardProfileId;

            var existing = await _service.GetById(leoId);
            if (existing != null)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardProfileId is exist" });

            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format" });

            if (dto.Weight == null || dto.Weight <= 15)
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15" });

            if (dto.LeopardTypeId == null || dto.LeopardTypeId <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardTypeId is required and must be > 0" });

            var leopard = new LeopardProfile
            {
                LeopardProfileId = leoId,
                LeopardTypeId = (int)dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = (double)dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = (DateTime)dto.ModifiedDate,
            };

            var result = await _service.Create(leopard);

            return CreatedAtAction(nameof(GetById), new { id = result }, dto);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDto dto)
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

            if (dto.LeopardName != null)
                existing.LeopardName = dto.LeopardName;

            if (dto.Characteristics != null)
                existing.Characteristics = dto.Characteristics;

            if (dto.CareNeeds != null)
                existing.CareNeeds = dto.CareNeeds;

            if (dto.Weight.HasValue)
            {
                if (dto.Weight <= 0)
                    return BadRequest(new { errorCode = "HB40001", message = "Weight must be > 15" });
                existing.Weight = (int)dto.Weight;
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
    }
}
