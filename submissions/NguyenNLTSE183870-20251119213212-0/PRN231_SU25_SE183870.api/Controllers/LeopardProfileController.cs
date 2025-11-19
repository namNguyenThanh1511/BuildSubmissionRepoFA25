using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service;
using Service.Dto;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE183870.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;
        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        [HttpGet]

        [Authorize(Roles = "administrator, moderator, developer, member")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var all = await _service.GetAll();
                return Ok(all);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("/api/[controller]/{id}")]

        [Authorize(Roles = "administrator, moderator, developer, member")]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var dto = await _service.GetById(id);
                if (dto == null)
                {
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
                }
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorCode = "HB50001", message = "Internal server error" });
            }
        }
        [HttpPost]

        [Authorize(Roles = "administrator, moderator")]

        public async Task<IActionResult> Create(CreateRequest request)
        {
            if (request.Weight < 15)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15" });
            }
          
            var pattern = @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (!Regex.IsMatch(request.LeopardName, pattern))
            {
                return BadRequest(new { errorCode = "HB40001", message = "Name must match regex" });
            }

            int result = await _service.Create(request);

            if (result > 0)
            {
                return Ok(new { message = " created successfully." });
            }
            else
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        

        [HttpPut("{id}")]

        [Authorize(Roles = "administrator, moderator")]

        public async Task<IActionResult> Update(int id, [FromBody] CreateRequest request)
        {
            try
            {
                if (request.Weight < 15)
                {
                    return BadRequest(new { errorCode = "HB40001", message = "Weight must be greater than 15" });
                }
                var pattern = @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
                if (!Regex.IsMatch(request.LeopardName, pattern))
                {
                    return BadRequest(new { errorCode = "HB40001", message = "Name must match regex" });
                }
                var result = await _service.Update(id, request);
                return Ok(new { message = " updated successfully."});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse("HB40401", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator, moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);

            if (!success)
                return NotFound(new { message = $" {id} not found." });

            return Ok(new { message = "deleted successfully." });
        }
        [HttpGet("search")]
        [Authorize(Roles = "administrator, moderator, developer, member")]
        [EnableQuery]

        public IActionResult Search()
        {
            var query = _service.SearchOData();
            return Ok(query);
        }
    }
}
