using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PRN231_SU_SE172732.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _leo;

        public LeopardProfileController(ILeopardProfileService handbagService)
        {
            _leo = handbagService;
        }

        //    5 => "administrator",
        //    6 => "moderator",
        //    7 => "developer",
        //    4 => "member",

        //[Authorize(Roles = "5,6,7,4")]
        [AllowAnonymous]
        // GET: api/handbags
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _leo.Get();
            return Ok(result);
        }

        //[Authorize(Roles = "5,6,7,4")]
        [AllowAnonymous]
        // GET: api/handbags/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _leo.Get(id);
            return Ok(result);
        }

        //[Authorize(Roles = "5,6")]
        [AllowAnonymous]
        // POST: api/handbags
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeopardProfileCreateModel model)
        {
            await _leo.Create(model);
            return Ok("Create succesfully");
        }

        //[Authorize(Roles = "5,6")]
        [AllowAnonymous]
        // POST: api/handbags
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileCreateModel model)
        {
            await _leo.Update(id, model);
            return Ok("Update succesfully");
        }

        //[Authorize(Roles = "5,6")]
        [AllowAnonymous]
        // DELETE: api/handbags/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _leo.Delete(id);
            return Ok("Delete succesfully");
        }
    }
}
