using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SU25Leopard.BusinessLogicLayer.Services;
using SU25Leopard.BusinessObject.DTO;

namespace SU25Leopard.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _leopardProfileService;
        public LeopardProfileController(LeopardProfileService leopardProfileService)
        {
            _leopardProfileService = leopardProfileService;
        }

        [HttpGet]
        [Authorize(Policy = "AllRoles")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _leopardProfileService.GetAllLeopardProfileAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AllRoles")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _leopardProfileService.GetLeopardProfileByIdAsync(id);
                if (result == null)
                    return NotFound(ErrorResponse.NotFound());

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrModerator")] // admin, mod only
        public async Task<IActionResult> Create([FromBody] LeopardProfileRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                    return BadRequest(ErrorResponse.BadRequest(firstError));
                }

                var result = await _leopardProfileService.AddLeopardProfileAsync(request);
                if (result == null)
                    return BadRequest(ErrorResponse.BadRequest("Invalid LeopardTypeId or LeopardProfile already exists"));

                return StatusCode(201, result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrModerator")] // admin, mod only
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                    return BadRequest(ErrorResponse.BadRequest(firstError));
                }

                var result = await _leopardProfileService.UpdateLeopardProfileAsync(request, id);
                if (result == null)
                    return NotFound(ErrorResponse.NotFound());

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrModerator")] // admin, mod only
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _leopardProfileService.DeleteLeopardProfileAsync(id);
                if (result == null)
                    return NotFound(ErrorResponse.NotFound());

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpGet("search")]
        [EnableQuery]
        [Authorize(Policy = "AllRoles")] // all roles with token
        public async Task<IActionResult> Search([FromQuery] string leopardName, [FromQuery] double weight)
        {
            try
            {
                var result = await _leopardProfileService.SearchLeopardProfile(leopardName, weight);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }
    }
}
