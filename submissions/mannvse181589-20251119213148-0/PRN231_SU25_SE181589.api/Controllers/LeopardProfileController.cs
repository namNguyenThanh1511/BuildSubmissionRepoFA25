using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace PRN231_SU25_SE181589.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileServices _services;
        public LeopardProfileController(LeopardProfileServices services)
        {
            _services = services;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _services.GetAll();
            return Ok(profiles);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var profile = await _services.GetById(id);
            if (profile == null)
                return NotFound(new { errorCode = "HB40401", message = "Not found" });
            return Ok(profile);
        }
    }
}
