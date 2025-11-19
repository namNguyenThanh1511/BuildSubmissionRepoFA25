using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PRN231_SU25_SE172411.api.Controllers
{
    [Route("api/leopardProfile")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetLeopardProfile()
        {
            try
            {
                var item = _service.GetLeopardProfile();
                return Ok(item);
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetLeopardProfileById(int id)
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
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            try
            {
                _service.AddLeopardProfile(item);
                return StatusCode(201, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize("AdminOnly")]
        public IActionResult Put(int id, [FromBody] UpdateLeopardProfileDTO updateLeopardProfileDTO)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            try
            {
                var existing = _service.GetLeopardProfileById(id);
                if (existing == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                updateLeopardProfileDTO.LeopardProfileId = id;
                _service.UpdateLeopardProfile(updateLeopardProfileDTO);

                return Ok(updateLeopardProfileDTO);
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


    }
}
