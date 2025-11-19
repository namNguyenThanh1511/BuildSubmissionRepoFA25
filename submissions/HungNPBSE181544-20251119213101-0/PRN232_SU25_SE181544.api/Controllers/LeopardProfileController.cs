using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN232_SU25_SE181544.BLL.DTOs;
using PRN232_SU25_SE181544.BLL.Services;

namespace PRN232_SU25_SE181544.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _service;

        public LeopardProfileController(LeopardProfileService leopardService)
        {
            _service = leopardService;
        }

        [HttpGet]
        [Authorize(Policy = "AllRoles")] // admin, mod, dev, member
        //[Authorize(Roles = "1,2")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AllRoles")] // admin, mod, dev, member
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] LeopardRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                    return BadRequest(ErrorResponse.BadRequest(firstError));
                }

                var result = await _service.Add(request);
                if (result == null)
                    return BadRequest(ErrorResponse.BadRequest("Invalid type or leopard already exists"));

                return StatusCode(201, result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrModerator")] // admin, mod only
        public async Task<IActionResult> Update(int id, [FromBody] LeopardRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                    return BadRequest(ErrorResponse.BadRequest(firstError));
                }

                var result = await _service.Update(request, id);
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
                var result = await _service.Delete(id);
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
        public async Task<IActionResult> Search([FromQuery] string? modelName, [FromQuery] double? weight)
        {
            if (weight == null) weight = 0;
            try
            {
                var result = await _service.Search(modelName, (double)weight);
                if (result == null)
                    return NotFound(ErrorResponse.NotFound());
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorResponse.InternalError());
            }
        }
    }
}
