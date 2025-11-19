using Microsoft.AspNetCore.Mvc;
using Services;

namespace PRN231_SU25_SE172213.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardTypeController : ControllerBase
    {
        private readonly ILeopardTypeService _service;

        public LeopardTypeController(ILeopardTypeService leopardTypeService)
        {
            _service = leopardTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _service.GetAll();

            if (brands == null || !brands.Any())
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "No Brands found"
                });
            }

            return Ok(brands);
        }

    }
}
