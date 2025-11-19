using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace PRN232_SU25_SE180586.Controllers
{
    public class LeopardController : ODataController
    {
        private readonly LeopardService _ser;
        public LeopardController(LeopardService leopardService)
        {
            _ser = leopardService;
        }

        [HttpGet]
      //  [Authorize(Policy = "AdminAndMod")]
        [Authorize(Policy = "DevAndMem")]
        [Route("api/LeopardProfile")]
        public async Task<IActionResult> GetAllLeopard()
        {
            var leopard = await _ser.GetAllLeopardAsync();
            return Ok(leopard);
        }

        [HttpGet]
    //    [Authorize(Policy = "AdminAndMod")]
        [Authorize(Policy = "DevAndMem")]
        [Route("api/LeopardProfile/{id}")]
        public async Task<IActionResult> GetLeopardById(int id)
        {
            var leopard = await _ser.GetProfileByIdAsync(id);
            if (leopard == null)
            {
                return NotFound();
            }
            return Ok(leopard);
        }

        [HttpDelete]
        [Authorize(Policy = "AdminAndMod")]
        [Route("api/LeopardProfile/{id}")]
        public async Task<IActionResult> DeleteLeopardAsync(int id)
        {
            var leopard = await _ser.GetProfileByIdAsync(id);
            if (leopard == null) 
            {
                return NotFound();
            }
            try
            {
                var ok = await _ser.DeleteLeopardAsync(id);
                if (ok) return Ok(leopard);
                else
                    return BadRequest("Failed to delete .");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
