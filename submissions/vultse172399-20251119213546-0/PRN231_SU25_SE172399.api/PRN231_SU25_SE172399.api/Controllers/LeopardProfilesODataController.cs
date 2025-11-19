using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Services;

namespace PRN231_SU25_SE172399.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfilesODataController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfilesODataController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [EnableQuery(MaxTop = 100, PageSize = 20)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{key}")]
        [Authorize]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize]
        [EnableQuery(MaxTop = 100, PageSize = 20)]
        public async Task<IActionResult> Search(
            [FromQuery] string? name = null,
            [FromQuery] double? weight = null,
            [FromQuery] double? minWeight = null,
            [FromQuery] double? maxWeight = null)
        {
            try
            {
                var items = await _service.SearchAsync(name, weight);

                var filteredItems = items.AsQueryable();

                if (minWeight.HasValue)
                    filteredItems = filteredItems.Where(item => item.Weight >= minWeight.Value);

                if (maxWeight.HasValue)
                    filteredItems = filteredItems.Where(item => item.Weight <= maxWeight.Value);

                return Ok(filteredItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error", details = ex.Message });
            }
        }
    }
}
