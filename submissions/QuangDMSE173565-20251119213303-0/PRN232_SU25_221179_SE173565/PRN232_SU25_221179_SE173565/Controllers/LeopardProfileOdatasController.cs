using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace PRN232_SU25_221179_SE173565.Controllers
{
    [Authorize(Policy = "Search")]
    [Route("api/LeopardProfile")]
    public class LeopardProfileOdatasController : ODataController
    {
        public readonly ILeopardProfileBL _service;

        public LeopardProfileOdatasController(ILeopardProfileBL service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? LeopardName, [FromQuery] string? Weight)
        {
            var result = await _service.SearchAsync(LeopardName, Weight);
            return Ok(result);
        }

    }
}
