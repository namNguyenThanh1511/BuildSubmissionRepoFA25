using BLL;
using BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace PRN231_SU25_SE181526.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public LeopardProfileController(IProfileService proService)
        {
            _profileService = proService;
        }

        [HttpGet]
        [Authorize(Roles = "administrator,moderator, developer, member")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _profileService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator,moderator, developer, member")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _profileService.GetItemByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
       [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> AddItem([FromBody] ProfileRequestDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(item.Weight <= 15)
            {
                throw new ArgumentNullException("Weight > 15");
            }
            var createdItem = await _profileService.AddItemAsync(item);
            var result = await _profileService.GetItemByIdAsync(createdItem.LeopardProfileId);
            return CreatedAtAction(nameof(GetItemById), new { id = result.LeopardProfileId }, result);
        }

        [HttpPut("{id}")]
       [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ProfileRequestDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (item.Weight <= 15)
            {
                throw new ArgumentNullException("Weight > 15");
            }
            var updatedItem = await _profileService.UpdateItemAsync(id, item);
            var result = await _profileService.GetItemByIdAsync(updatedItem.LeopardProfileId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _profileService.DeleteItemAsync(id);
            return Ok(new { message = "Delete Successfully" });
        }

        [HttpGet("search")]
        [EnableQuery]
        [Authorize(Roles = "administrator,moderator, developer, member")]
        public async Task<IActionResult> Search([FromQuery] string? LeopardName,[FromQuery] double? Weight)
        {
            try
            {
                var all = await _profileService.GetAllItemsAsync();

                var filtered = all
                    .Where(h =>
                        (string.IsNullOrEmpty(LeopardName) || h.LeopardName.Contains(LeopardName, StringComparison.OrdinalIgnoreCase)) &&
                                (string.IsNullOrEmpty(Weight.ToString()) || h.Weight == Weight)
                        );

                return Ok(filtered);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while processing your request." });
            }
        }
    

    }
}
