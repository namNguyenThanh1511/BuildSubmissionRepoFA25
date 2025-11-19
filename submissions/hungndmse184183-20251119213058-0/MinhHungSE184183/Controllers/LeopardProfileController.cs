using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs;
using Services.Interface;

namespace MinhHungSE184183.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardService _service;

        public LeopardProfileController(ILeopardService leopardService)
        {
            _service = leopardService;
        }
        [HttpGet]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetAll()
        {
            var leopards = await _service.GetAllAsync();
            return Ok(leopards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var leopard = await _service.GetByIdAsync(id);
            if (leopard == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
            return Ok(leopard);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] CreateLeopardDTO dto)
        {
            var (created, error) = await _service.CreateAsync(dto);
            if (error != null)
            {
                var parts = error.Split(':');
                return BadRequest(new { errorCode = parts[0], message = parts[1] });
            }

            return CreatedAtAction(nameof(GetById), new { id = created!.LeopardProfileId }, created);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateLeopardDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { code = "HB40001", message = "Missing or invalid input" });

            var result = await _service.UpdateAsync(id, dto);
            if (result.Updated != null) return Ok(result.Updated);
            if (!string.IsNullOrWhiteSpace(result.Error))
                return NotFound(new { code = "HB40401", message = result.Error });

            return BadRequest(new { code = "HB40001", message = "Missing or invalid input" });
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { code = "HB40401", message = "Resource not found" });
            return Ok();
        }
    }
}
