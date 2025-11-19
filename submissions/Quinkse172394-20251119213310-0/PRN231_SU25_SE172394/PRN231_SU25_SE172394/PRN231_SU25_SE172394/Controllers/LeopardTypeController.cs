using Microsoft.AspNetCore.Mvc;
using Services;

namespace PRN231_SU25_SE172394.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeopardTypeController : ControllerBase
    {
        private readonly ILeopardTypeService _typeService;

        public LeopardTypeController(ILeopardTypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeopardTypes()
        {
            var leopardTypes = await _typeService.GetAll();

            if (leopardTypes == null || !leopardTypes.Any())
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "No LeopardType found"
                });
            }

            return Ok(leopardTypes);
        }

    }
}
