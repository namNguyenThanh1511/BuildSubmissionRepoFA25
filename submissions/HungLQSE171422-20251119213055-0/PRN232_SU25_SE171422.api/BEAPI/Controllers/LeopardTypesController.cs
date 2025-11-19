using Microsoft.AspNetCore.Mvc;
using Services;

namespace BEAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LeopardTypesController : ControllerBase
    {
        private readonly ILeopardTypeService _typeService;

        public LeopardTypesController(ILeopardTypeService typeService)
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
