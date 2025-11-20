using BusinessLogicLayer.Interface;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using static BusinessLogicLayer.Service.LeopardProfileService;

namespace PRN231_SU25_HE180905.api.Controller
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnly")]
        [ProducesResponseType(typeof(IEnumerable<LeopardProfile>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetAll()
        {
            var list = _service.GetAllProfiles();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnly")]
        [ProducesResponseType(typeof(LeopardProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetById(int id)
        {
            try
            {
                var h = _service.GetProfileById(id);
                return Ok(h);
            }
            catch (ServiceException ex) when (ex.ErrorCode == "HB40401")
            {
                return NotFound(new { errorCode = ex.ErrorCode, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "FullAccess")]
        [ProducesResponseType(typeof(LeopardProfile), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Create([FromBody] LeopardProfile profile)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            try
            {
                var created = _service.CreateProfile(profile);
                return CreatedAtAction(nameof(GetById), new { id = created.LeopardProfileId }, created);
            }
            catch (ServiceException ex) when (ex.ErrorCode == "HB40001")
            {
                return BadRequest(new { errorCode = ex.ErrorCode, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "FullAccess")]
        [ProducesResponseType(typeof(LeopardProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Update(int id, [FromBody] LeopardProfile profile)
        {
            if (id != profile.LeopardProfileId)
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            if (!ModelState.IsValid)
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            try
            {
                var updated = _service.UpdateProfile(profile);
                return Ok(updated);
            }
            catch (ServiceException ex) when (ex.ErrorCode == "HB40401")
            {
                return NotFound(new { errorCode = ex.ErrorCode, message = ex.Message });
            }
            catch (ServiceException ex) when (ex.ErrorCode == "HB40001")
            {
                return BadRequest(new { errorCode = ex.ErrorCode, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "FullAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteProfile(id);
                return Ok();
            }
            catch (ServiceException ex) when (ex.ErrorCode == "HB40401")
            {
                return NotFound(new { errorCode = ex.ErrorCode, message = ex.Message });
            }
        }

        [HttpGet("search")]
        [EnableQuery]
        [Authorize(Policy = "ReadOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Search([FromQuery] string leopardName, [FromQuery] string weight)
        {
            var query = _service.SearchProfile(leopardName);
            return Ok(query);
        }
    }
}
