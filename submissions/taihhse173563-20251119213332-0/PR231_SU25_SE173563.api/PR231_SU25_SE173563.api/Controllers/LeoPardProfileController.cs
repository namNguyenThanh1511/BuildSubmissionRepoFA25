using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service;
using Service.Model;

namespace PRN231_SU25_SE._173563api.Controllers
{
    [Route("api/LeoPardProfile")]
    [ApiController]
    public class LeoPardProfileController : ODataController
    {
        private readonly LeopardService _leoPardService;
        public LeoPardProfileController(LeopardService leoPardService)
        {
            _leoPardService = leoPardService;

        }

        [Authorize]
        [HttpGet]
       public async Task<IActionResult> GetAllLeos()
        {
            var handbags = await _leoPardService.GetAllLeoAsync();
            return Ok(handbags);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeoById(int id)
        {
            var handbag = await _leoPardService.GetLeoByIdAsync(id);
            if (handbag == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
            return Ok(handbag);
        }
        [Authorize(Roles = "5,6")]
        [HttpPost]
        public async Task<IActionResult> AddLeo([FromBody] LeopardCreateDTO handbag)
        {
            try
            {
                await _leoPardService.AddLeoAsync(handbag);
                return Ok("Add succesfully");
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }
        }
        [Authorize(Roles = "5,6")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeo([FromRoute] int id, [FromBody] LeopardCreateDTO handbag)
        {
            try
            {
                await _leoPardService.UpdateLeoAsync(id, handbag);
                return Ok("Update suceesfully");
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }
            
        }
        [Authorize(Roles = "5,6")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeo([FromRoute] int id)
        {
            try
            {
                await _leoPardService.DeleteLeoAsync(id);
                return Ok("Delete success");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { errorCode = "HB40401", message = "Resource not found" });
            }   
        }
        [Authorize]
        [EnableQuery]
        [HttpGet("search")]     
        public async Task<IActionResult> Search(string? leoPardName, double? weight)
        {
            var result = await _leoPardService.Search(leoPardName, weight);
            return Ok(result);
        }

    }
}
