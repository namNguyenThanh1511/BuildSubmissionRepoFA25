using PRN231_SU25_SE173164.bll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_SU25_SE173164.bll.DTOs;
using PRN231_SU25_SE173164.dal.Entities;

namespace PRN231_SU25_SE173164.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly IService _service;
        public LeopardProfileController(IService service)
        {
            _service = service;
        }

        [Authorize(Roles = "5,6,7,4")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeopardProfileAsync()
        {
            return Ok(await _service.GetAllLeopardProfile());
        }

        [Authorize(Roles = "5,6,7,4")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeopardProfileByIdAsync(int id)
        {
            var leopardProfile = await _service.GetLeopardProfileByIdAsync(id);
            return Ok(leopardProfile);
        }


        [Authorize(Roles = "5,6")]
        [HttpPost]
        public async Task<IActionResult> CreateHandbagAsync([FromBody] LeopardProfileDTO model)
        {
            await _service.CreateLeopardProfileAsync(model);
            return NoContent();
        }

        [Authorize(Roles = "5,6")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateLeopardProfileAsync(int id, [FromBody] LeopardProfileUpdateDTO model)
        {
            await _service.UpdateLeopardProfileAsync(id, model);
            return NoContent();
        }

        [Authorize(Roles = "5,6")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeopardProfileAsync(int id)
        {
            await _service.DeleteLeopardProfileAsync(id);
            return NoContent();
        }
    }
}
