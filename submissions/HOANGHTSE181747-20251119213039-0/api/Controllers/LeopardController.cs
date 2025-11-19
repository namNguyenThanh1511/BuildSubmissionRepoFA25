using BLL.Service;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LeopardController : ODataController
    {

        private readonly ILeopardService _service;
        public LeopardController(ILeopardService service)
        {
            _service = service;
        }
        [EnableQuery]
        [Authorize(Policy = "AdminDevMember")]

        [HttpGet("/api/LeopardProfile")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetLeopardProfiles()
        {
            try
            {
                var players = await _service.GetLeopardProfiles();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }



        [HttpGet("/api/LeopardProfile/{id}")]
        [Authorize(Policy = "AdminDevMember")]

        public async Task<ActionResult<LeopardProfile>> GetPlayer(string id)
        {
            try
            {
                var player = await _service.GetLeopardProfile(id);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }
        [HttpPost("/api/LeopardProfile")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<LeopardProfile>> AddPlayer([FromBody] LeopardProfile player)
        {
            try
            {
                await _service.AddLeopardProfile(player);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpPut("/api/LeopardProfile/{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<LeopardProfile>> UpdatePlayer(string id, [FromBody] LeopardProfile player)
        {
            try
            {
                await _service.UpdateLeopardProfile(player);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [HttpDelete("/api/LeopardProfile/{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<LeopardProfile>> DeletePlayer(string id)
        {
            try
            {
                var deletedPlayer = await _service.GetLeopardProfile(id);
                await _service.DeleteLeopardProfile(id);
                return Ok(deletedPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

    }
}
