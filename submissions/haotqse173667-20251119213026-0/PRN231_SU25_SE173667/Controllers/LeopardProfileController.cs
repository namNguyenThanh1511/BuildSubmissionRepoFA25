using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using BusinessLogic.ModalViews;

namespace PRN231_SU25_SE173667.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
       
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
    
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            if (result == null)
                return NotFound(new { errorCode = "HB40401", message = "Handbag not found" });
            return Ok(result);
        }

        [HttpPost]
       
        public async Task<IActionResult> Create([FromBody] LeopardProfileRequest request)
        {
            if (!Regex.IsMatch(request.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

          

            var success = await _service.Create(request);
            return success ? StatusCode(201, new { message = "Create successfully" }) : StatusCode(500, new { errorCode = "HB50001", message = "Create failed" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileRequest request)
        {
            var success = await _service.Update(id, request);
            if (!success)
                return NotFound(new { errorCode = "HB40401", message = "Handbag not found" });
            return Ok();
        }

        [HttpDelete("{id}")]
        
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);
            if (!success)
                return NotFound(new { errorCode = "HB40401", message = "Handbag not found" });
            return Ok();
        }

        //[HttpGet("search")]
        //[Authorize] // All roles with token
        //public async Task<IActionResult> Search([FromQuery] string? modelName, [FromQuery] string? material)
        //{
        //    //var result = await _service.Search(modelName, material);
        //    //return Ok(result);
        //}
    }
}
