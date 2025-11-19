using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebAPI.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfileController : ODataController
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { errorCode = "HB40401", message = "LeopardProfile not found" });

            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] LeopardProfile leopardProfile)
        {
            //var newId = await _service.GetAllAsync().ContinueWith(t => t.Result.Max(h => h.LeopardProfileId) + 1);
            //leopardProfile.LeopardProfileId = newId;
            var (isSuccess, errorCode, errorMsg) = await _service.AddAsync(leopardProfile);
            if (!isSuccess)
                return BadRequest(new { errorCode, message = errorMsg });

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfile leopardProfile)
        {
            leopardProfile.LeopardProfileId = id;
            var (isSuccess, errorCode, errorMsg) = await _service.UpdateAsync(leopardProfile);
            if (!isSuccess)
            {
                if (errorCode == "HB40401")
                    return NotFound(new { errorCode, message = errorMsg });
                return BadRequest(new { errorCode, message = errorMsg });
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var (isSuccess, errorCode, errorMsg) = await _service.DeleteAsync(id);
            if (!isSuccess)
                return NotFound(new { errorCode, message = errorMsg });
            return Ok();
        }

        [EnableQuery]
        [HttpGet("search")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult SearchGrouped([FromQuery] string? LeopardName, [FromQuery] double? Weight)
        {
            var results = _service.Search(LeopardName, Weight);
            return Ok(results);
        }
    }
}
