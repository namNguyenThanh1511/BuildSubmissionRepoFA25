using Business.Interfaces;
using Business.Models;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE173526.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeoPardService _service;

        public LeopardProfileController(ILeoPardService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _service.GetById(id);
            if (result == null)
                return NotFound(new { errorCode = "HB40401", message = "Not found" });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Create([FromBody] LeoPardRequestDTO dto)
        {
            var namePattern = @"^([A-z0-9][a-zA-Z0-9]*\s)*([A-z0-9][a-zA-Z0-9]*)$";
            if (string.IsNullOrWhiteSpace(dto.LeopardName) || !System.Text.RegularExpressions.Regex.IsMatch(dto.LeopardName, namePattern))
            {
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid. It must match the required pattern." });
            }

            if (dto.Weight <= 15)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15." });
            }

            var leoPard = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                LeopardTypeId = dto.LeopardTypeId,
                ModifiedDate = DateTime.Now
            };

            _service.Create(leoPard);

            return StatusCode(201, leoPard);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Update(int id, [FromBody] LeoPardRequestDTO dto)
        {
            var existing = _service.GetAll().FirstOrDefault(h => h.LeopardProfileId == id);
            if (existing == null)
                return NotFound(new { errorCode = "HB40401", message = "LeoPard profile not found" });

            var namePattern = @"^([A-z0-9][a-zA-Z0-9]*\s)*([A-z0-9][a-zA-Z0-9]*)$";
            if (string.IsNullOrWhiteSpace(dto.LeopardName) || !System.Text.RegularExpressions.Regex.IsMatch(dto.LeopardName, namePattern))
            {
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid. It must match the required pattern." });
            }

            if (dto.Weight <= 15)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15." });
            }

            var updatedLeoPard = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                LeopardTypeId = dto.LeopardTypeId,
                ModifiedDate = DateTime.Now
            };

            _service.Update(updatedLeoPard);
            return Ok(updatedLeoPard);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Delete(int id)
        {
            var existing = _service.GetById(id);
            if (existing == null)
                return NotFound(new { errorCode = "HB40401", message = "Not found" });

            _service.Delete(id);
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult Search(string? leopardName = "", double? weight = null)
        {
            var result = _service.Search(leopardName ?? "", weight);
            return Ok(result);
        }
    }
}
