using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services;

namespace PRN231_SU25_SE172399.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly ILeopardProfileService service;

        public LeopardProfilesController(ILeopardProfileService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var item = await service.GetByIdAsync(id);
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
        [Authorize("ModeratorOnly")]
        public async Task<IActionResult> Post([FromBody] LeopardProfileDTO leopard)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            try
            {
                await service.CreateAsync(leopard);
                return StatusCode(201, leopard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize("AdminOnly")]
        [Authorize("ModeratorOnly")]
        public async Task<IActionResult> Put(int id, [FromBody] LeopardProfileDTO leopard)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.First().Errors.First().ErrorMessage;
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            try
            {
                var existing = await service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                leopard.LeopardProfileId = id;
                await service.UpdateAsync(leopard);

                return Ok(leopard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize("AdminOnly")]
        [Authorize("ModeratorOnly")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

                await service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] double? weight, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var all = await service.SearchAsync(name, weight);
                var totalCount = all.Count;

                var pagedResult = all
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    data = pagedResult,
                    totalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = ex.Message });
            }
        }
    }
}
