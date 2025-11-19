using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace PRN232_SU25_SE183936.api.Controllers
{
    [Authorize(Roles = "administrator,moderator,developer,member")]

    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly Su25leopardDbContext _context;

        public LeopardProfilesController(Su25leopardDbContext context)
        {
            _context = context;
        }

        // GET: api/LeopardProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetLeopardProfiles()
        {
            return await _context.LeopardProfiles.ToListAsync();
        }

        // GET: api/LeopardProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeopardProfile>> GetLeopardProfile(int id)
        {
            var leopardProfile = await _context.LeopardProfiles.FindAsync(id);

            if (leopardProfile == null)
            {
                return NotFound();
            }

            return leopardProfile;
        }

        //PUT: api/LeopardProfiles/5
        [Authorize(Roles = "administrator,moderator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeopardProfile(int id, LeopardProfile leopardProfile)
        {
            if (id != leopardProfile.LeopardProfileId)
            {
                return BadRequest(new { errorCode = "HB40001", message = "ID mismatch" });
            }

            _context.Entry(leopardProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeopardProfileExists(id))
                {
                    return NotFound(new { errorCode = "HB40401", message = "ID not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/LeopardProfiles
        [Authorize(Roles = "administrator,moderator")]
        [HttpPost]
        public async Task<ActionResult<LeopardProfile>> PostLeopardProfile(LeopardProfile leopardProfile)
        {
            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

            if (!regex.IsMatch(leopardProfile.LeopardName ?? ""))
                return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid" });
            
            if (leopardProfile.Weight <= 15)
                return BadRequest(new { errorCode = "HB40001", message = "weight must be greater than 15" });


            _context.LeopardProfiles.Add(leopardProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeopardProfile", new { id = leopardProfile.LeopardProfileId }, leopardProfile);
        }

        // DELETE: api/LeopardProfiles/5
        [Authorize(Roles = "administrator,moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeopardProfile(int id)
        {
            var leopardProfile = await _context.LeopardProfiles.FindAsync(id);
            if (leopardProfile == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Leopard profile not found" });
            }

            _context.LeopardProfiles.Remove(leopardProfile);
            await _context.SaveChangesAsync();


            return NoContent();
        }

        // GET: api/Handbags/search?modelName=X&material=Y
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? leopardName, double? weight)
        {
            var query = _context.LeopardProfiles
                .Include(h => h.LeopardType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(leopardName))
                query = query.Where(h => h.LeopardName.Contains(leopardName));

            if (weight != null)
                query = query.Where(h => h.Weight.Equals(weight));

            var result = await query
                .GroupBy(h => h.LeopardType != null ? h.LeopardType.LeopardTypeName : "Unknown")
                .Select(g => new
                {
                    LeopardType = g.Key,
                    LeopardProfile = g.Select(h => new
                    {
                        h.LeopardProfileId,
                        h.LeopardName,
                        h.LeopardTypeId,
                        h.CareNeeds,
                        h.Weight,
                        h.Characteristics,
                        h.ModifiedDate,
                        h.LeopardType
                    }).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

        private bool LeopardProfileExists(int id)
        {
            return _context.LeopardProfiles.Any(e => e.LeopardProfileId == id);
        }
    }
}
