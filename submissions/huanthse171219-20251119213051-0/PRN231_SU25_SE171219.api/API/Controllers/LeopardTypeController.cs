using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeopardTypeController : ControllerBase
    {
        private readonly LeopardTypeService _typeService;

        public LeopardTypeController(LeopardTypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _typeService.GetAll();

            if (items == null || !items.Any())
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "No Leopard Types found"
                });
            }

            return Ok(items);
        }

    }
}

