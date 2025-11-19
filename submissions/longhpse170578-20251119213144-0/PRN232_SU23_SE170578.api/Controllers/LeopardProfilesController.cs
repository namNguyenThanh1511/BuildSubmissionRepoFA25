using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN232_SU23_SE170578.api.Data;
using PRN232_SU23_SE170578.api.Models;
using PRN232_SU23_SE170578.api.Services;

namespace PRN232_SU23_SE170578.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly ILeopardProfileService _service;
        public LeopardProfilesController(ILeopardProfileService service)
        {
            _service = service;
        }

        // GET: api/LeopardProfiles
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetLeopardProfile()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // GET: api/LeopardProfiles/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<LeopardProfile>> GetLeopardProfile(int id)
        {
            try
            {
                var result = await _service.GetOne(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // PUT: api/LeopardProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOrStaff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeopardProfile(int id, LeopardProfile leopardProfile)
        {
            try
            {
                leopardProfile.LeopardProfileId = id;
                var result = await _service.Update(leopardProfile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // POST: api/LeopardProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOrStaff")]
        [HttpPost]
        public async Task<ActionResult<LeopardProfile>> PostLeopardProfile(LeopardProfile leopardProfile)
        {
            try
            {
                var result = await _service.Add(leopardProfile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // DELETE: api/LeopardProfiles/5

        [Authorize(Policy = "AdminOrStaff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeopardProfile(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

     
    }
}
