using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAO.Models;
using Service;
using DAO.DTO;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

namespace PRN231_SU25_SE183026.api.Controllers
{
    [Route("api/LeopardProfile")]
    [Authorize]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly ILeopardProfileService _context;

        public LeopardProfilesController(ILeopardProfileService context)
        {
            _context = context;
        }

        // GET: api/LeopardProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetLeopardProfiles()
        {
            return await _context.GetAllAsync();
        }

        // GET: api/LeopardProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeopardProfile>> GetLeopardProfile(int id)
        {
            var a = await _context.GetByIdAsync(id);

            if (a == null)
            {
                return NoContent();
            }

            return a;
        }

        // PUT: api/LeopardProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "FullAccess")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeopardProfile(int id, LeopardProfile leopardProfile)
        {
            if (id != leopardProfile.LeopardProfileId)
            {
                return NotFound(new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            }


            await _context.UpdateAsync(leopardProfile);

            return NoContent();
        }

        // POST: api/LeopardProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "FullAccess")]
        [HttpPost]
        public async Task<ActionResult<LeopardProfile>> PostLeopardProfile(CreateProfile leopardProfile)
        {
            await _context.CreateAsync(leopardProfile);

            return NoContent();
        }

        // DELETE: api/LeopardProfiles/5
        [Authorize(Policy = "FullAccess")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeopardProfile(int id)
        {
            await _context.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("search")]
        [EnableQuery]
        public async Task<IActionResult> Search()
        {
            var query = (await _context.GetAllAsync()).AsQueryable();

            return Ok(query);
        }
    }
}
