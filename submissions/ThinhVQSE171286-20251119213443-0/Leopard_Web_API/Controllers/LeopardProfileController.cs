using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace Leopard_Web_API.Controllers
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
            var filer = new LeopardProfileFilter
            {
              LeopardName   = "",
              weight = 0
            };
            var list = _service.GetAllQueryable(filer);
            return Ok(list);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] LeopardProfileFilter filter)
        {
            var results = _service.GetAllQueryable(filter);
            return Ok(results);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] LeopardProfileDTO dto)
        {
      
            if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
                !Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format" });

            if (dto.Weight == null || dto.Weight <= 15)
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15" });


            var leopard = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                LeopardTypeId = dto.LeopardTypeId,
                CareNeeds = dto.CareNeeds,
                Characteristics = dto.Characteristics,
                ModifiedDate = dto.ModifiedDate,
            };
            var result = await _service.Create(leopard);

            return CreatedAtAction(nameof(GetById), new { id = result }, dto);
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
        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLeopardProfileDTO dto)
        {
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

            if (dto.CareNeeds != null)
                existing.CareNeeds = dto.CareNeeds;

            if (dto.Weight.HasValue)
            {
                if (dto.Weight <= 15)
                    return BadRequest(new { errorCode = "HB40001", message = "Weight must be > 15" });
                existing.Weight = (double)dto.Weight;
            }
            if (dto.Characteristics  != null)
                existing.Characteristics = dto.Characteristics;

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