using Microsoft.AspNetCore.Mvc;
using Services;

namespace BEAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardTypesController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public LeopardTypesController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeopardTypes()
        {
            var LeopardTypes = await _typeService.GetAll();

            if (LeopardTypes == null || !LeopardTypes.Any())
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "No LeopardTypes found"
                });
            }

            return Ok(LeopardTypes);
        }

    }
}
