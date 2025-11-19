using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;

namespace PRN231_SU25_SE172399.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardTypesController : ControllerBase
    {
        private readonly ILeopardTypeService service;

        public LeopardTypesController(ILeopardTypeService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LeopardType>>> Get()
        {
            try
            {
                var leopards = await service.GetAllAsync();
                return Ok(leopards);
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }
    }
}
