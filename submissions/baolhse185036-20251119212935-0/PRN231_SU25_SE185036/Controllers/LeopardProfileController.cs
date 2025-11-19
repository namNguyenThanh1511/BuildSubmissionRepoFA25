using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU25_SE185036.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "5,6,7,4")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        [Authorize(Roles = "5,6,7,4")]
        public IActionResult Get(int id)
        {
            var leopardProfile = _service.Get(id);
            return leopardProfile == null ? NotFound() : Ok(leopardProfile);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "5,6")]
        public IActionResult Delete(int id)
        {
            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            _service.Delete(id);
            return Ok();
        }
    }
}
