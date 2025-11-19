using BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repo;

namespace PRN232_SU25_SE160890.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfile _leopardProfile;

        public LeopardProfileController(ILeopardProfile leopardProfile)
        {
            _leopardProfile = leopardProfile;
        }
        [EnableQuery]
        [Authorize(Policy = "AdminOrMorderator")]
        [HttpGet("api/leopard")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetLeopard()
        {
            try
            {
                var leo = await _leopardProfile.GetLeopard();
                return Ok(leo);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }
    }
}
