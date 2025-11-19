using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace PRN231_SU25_SE173524.api.Controllers
{
    [Authorize] 
    [Route("api/LeopardProfile")]
    public class LeopardODataController : Controller
    {
        public readonly ILeopardProfileBL _leopard;

        public LeopardODataController(ILeopardProfileBL leopard)
        {
            _leopard = leopard;
        }

        [EnableQuery]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] double weight)
        {
            var result = await _leopard.SearchAsync(name, weight);
            return Ok(result);
        }
    }
}
