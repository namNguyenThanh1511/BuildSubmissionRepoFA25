using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models;

namespace PRN231_SU25_SE173519.api.Controller
{
    [Route("api/leoprofile")]
    [ApiController]
    public class LeoProfileController : ControllerBase
    {
        private readonly ILeoProfileService _service;

        public LeoProfileController(ILeoProfileService service)
        {
            _service = service;
        }

        [Authorize(Roles = "4,5,6,7")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.Get(pageIndex, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "4,5,6,7")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.Get(id);
            return Ok(result);
        }

        [Authorize(Roles = "5,6")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeoCreateModel model)
        {
            await _service.Create(model);
            return Ok("Create succesfully");
        }

        [Authorize(Roles = "5,6")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeoCreateModel model)
        {
            await _service.Update(id, model);
            return Ok("Update succesfully");
        }

        [Authorize(Roles = "5,6")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("Delete succesfully");
        }

        [Authorize(Roles = "4,5,6,7")]
        [HttpGet("search")]
        public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] double? weight,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await _service.Search(name, weight, pageIndex, pageSize);
            return Ok(result);
        }
    }
}
