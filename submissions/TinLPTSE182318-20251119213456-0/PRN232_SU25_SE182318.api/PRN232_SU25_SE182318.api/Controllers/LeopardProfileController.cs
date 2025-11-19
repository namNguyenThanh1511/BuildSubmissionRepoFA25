using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE182318.api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetLeopardProfiles();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetLeopardProfileById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> Create(LeopardProfileDTO dto)
        {
            var result = await _service.CreateLeopardProfile(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.LeopardProfileId }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> Update(int id, LeopardProfileUpdateDTO dto)
        {
            var success = await _service.UpdateLeopardProfile(id, dto);
            if (!success) return NotFound(new
            {
                errorCode = "HB40401",
                message = "Profile not found"
            });
            return Ok("Update succesfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteLeopardProfile(id);
            if (!success) return NotFound(new
            {
                errorCode = "HB40401",
                message = "Profile not found"
            });
            return Ok("Delete succesfully");
        }

        [HttpGet("search")]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> Search([FromQuery] string leopardName, [FromQuery] double weight)
        {
            var result = await _service.SearchLeopardProfile(leopardName, weight);
            return Ok(result);
        }
    }


}
