using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN232_SU25_SE180262.api.Controllers
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
        [Authorize(Policy = "ReadOnly")]
        [ProducesResponseType(typeof(IEnumerable<LeopardProfile>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "ReadOnly")]
        [ProducesResponseType(typeof(LeopardProfile), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { errorCode = "HB40401", message = "LeopardProfile not found" });

            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = "FullAccess")]
        [ProducesResponseType(typeof(LeopardProfile), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Create([FromBody] LeopardProfile dto)
        {
            if (dto == null)
                return BadRequest(new { errorCode = "HB40001", message = "LeopardProfile data is required" });

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById),
                                       new { id = created.LeopardProfileId },
                                       created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorCode = "HB40001", message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "FullAccess")]
        [ProducesResponseType(typeof(LeopardProfile), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfile dto)
        {
            if (dto == null || id != dto.LeopardProfileId)
                return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardProfile data" });

            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorCode = "HB40001", message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { errorCode = "HB40401", message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "FullAccess")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { errorCode = "HB40401", message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpGet("search")]
        [Authorize(Policy = "ReadOnly")]
        [ProducesResponseType(typeof(IEnumerable<LeopardProfile>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Search([FromQuery] string? leopardName,
                                                [FromQuery] double? weight)
        {
            var results = await _service.SearchAsync(leopardName, weight);
            return Ok(results);
        }
    }
}
