using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PRN231_SU25_SE172412.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfilesController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var items = _service.GetLeopardProfiles();
                return Ok(items);
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var item = _service.GetLeopardProfileById(id);
                if (item == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                return Ok(item);
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpPost]
        [Authorize("AdminOnly")]
        public IActionResult Post([FromBody] LeopardProfileDTO item)
        {
            if (item == null || !ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            try
            {
                _service.CreateLeopardProfile(item);
                return StatusCode(201, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize("AdminOnly")]
        public IActionResult Put(int id, [FromBody] LeopardProfileDTO item)
        {
            if (item == null || !ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            try
            {
                var existing = _service.GetLeopardProfileById(id);
                if (existing == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                item.LeopardProfileId = id;
                _service.UpdateLeopardProfile(item);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize("AdminOnly")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existing = _service.GetLeopardProfileById(id);
                if (existing == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                _service.RemoveLeopardProfile(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult Search(string? LeopardName, double? Weight)
        {
            try
            {
                var result = _service.SearchLeopardProfiles(LeopardName, Weight);
                return Ok(new
                {
                    data = result,
                    totalCount = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }
    }
}
