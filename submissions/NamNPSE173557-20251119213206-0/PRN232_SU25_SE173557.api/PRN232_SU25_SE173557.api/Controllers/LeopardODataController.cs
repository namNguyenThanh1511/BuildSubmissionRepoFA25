using BLL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace PRN232_SU25_SE173557
{
    [Authorize] // tất cả role đều dùng được
    [Route("api/LeopardProfile")]
    public class LeopardODataController : ODataController
    {
        public readonly ILeopardBL leopardBL;

        public LeopardODataController(ILeopardBL systemAccountService)
        {
            leopardBL = systemAccountService;
        }

        [EnableQuery]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? leopardName)
        {
            var result = await leopardBL.SearchAsync(leopardName);
            return Ok(result);
        }

    }
}
