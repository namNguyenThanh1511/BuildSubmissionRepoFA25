using BusinessLogic.Service;
using DataAccess.Models;
using DataAccess.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeopardProfileController : ControllerBase
    {

        private readonly ILeopardProfileService _leopardService;

        public LeopardProfileController(ILeopardProfileService leopardService)
        {
            _leopardService = leopardService;
        }

        [HttpGet]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetAllLeopards()
        {
            try
            {
                var leopards = await _leopardService.GetAllLeopardsAsync();
                return Ok(leopards);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<ActionResult<LeopardProfile>> GetLeopardById(int id)
        {
            try
            {
                var leopard = await _leopardService.GetLeopardByIdAsync(id);
                if (leopard == null)
                {
                    return StatusCode(404, new
                    {
                        ErrorCode = "HB40401",
                        Message = "Resource not found"
                    });
                }
                return Ok(leopard);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> CreateLeopard([FromBody] LeopardProfileRequest leopard)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .FirstOrDefault();

                    return BadRequest(new
                    {
                        ErrorCode = "HB40001",
                        Message = firstError ?? "Invalid input"
                    });
                }
                var result = await _leopardService.CreateLeopardProfileAsync(leopard);
                if (result)
                {
                    return StatusCode(201, new { message = "Create Successfully" });
                }
                return BadRequest(new
                {
                    ErrorCode = "HB40001",
                    Message = "Invalid input"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> UpdateLeopard(int id, [FromBody] LeopardProfileRequest leopard)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var firstError = ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .FirstOrDefault();
                    return BadRequest(new
                    {
                        ErrorCode = "HB40001",
                        Message = firstError ?? "Invalid input"
                    });
                }
                var result = await _leopardService.UpdateLeopardProfileAsync(id, leopard);
                if (!result)
                {
                    return StatusCode(404, new
                    {
                        ErrorCode = "HB40401",
                        Message = "Resource not found"
                    });
                }
                return Ok(new { message = "Update Successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> DeleteLeopard(int id)
        {
            try
            {
                var result = await _leopardService.DeleteLeopardProfileAsync(id);
                if (!result)
                {
                    return StatusCode(404, new
                    {
                        ErrorCode = "HB40401",
                        Message = "Resource not found"
                    });
                }
                return Ok(new { message = "Delete Successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }
        }

        [HttpGet("search")]
        [EnableQuery]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> SearchLeopard([FromQuery] string? LeopardName, [FromQuery] double? Weight)
        {
            try
            {
                var leopards = await _leopardService.SearchLeopardProfileAsync(LeopardName, Weight);
                return Ok(leopards);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    ErrorCode = "HB50001",
                    Message = "Internal server error"
                });
            }
        }
    }
}
