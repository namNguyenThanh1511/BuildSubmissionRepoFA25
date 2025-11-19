using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184714.api.Models;
using Repositories.Dto;
using Repositories.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184714.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly LeopardProfileService _service;
        private readonly LeopardTypeService _typeService;

        public LeopardProfilesController() 
        {
            _service = new LeopardProfileService();
            _typeService = new LeopardTypeService();
        }

        [HttpGet]
        [Authorize(Roles = "4,5,6,7")]
        public async Task<ActionResult<List<LeopardProfile>?>> GetLeopardProfiles()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "4,5,6,7")]
        public async Task<ActionResult<LeopardProfile>> GetLeopardProfile(int id)
        {
            var leopardProfile = await _service.GetById(id);

            if (leopardProfile == null)
            {
                return NotFound(ErrorCodeModel.NotFound());
            }

            return Ok(leopardProfile);
        }

        [HttpGet("search")]
        [Authorize]
        [EnableQuery]
        public async Task<ActionResult<List<LeopardProfile>?>> SearchLeopardProfiles()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> PutLeopardProfile(int id, [FromBody] UpdateLeopardProfileDto leopardProfile)
        {
            if (id != leopardProfile.LeopardProfileId)
            {
                return BadRequest(ErrorCodeModel.Invalid());
            }

            var item = await _service.GetById(id);
            if (item == null)
            {
                return NotFound(ErrorCodeModel.NotFound());
            }

            try
            {
                
                await _service.Update(leopardProfile);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LeopardProfileExists(id))
                {
                    return NotFound(ErrorCodeModel.NotFound());
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "5,6")]

        public async Task<ActionResult<LeopardProfile>> PostLeopardProfile([FromBody] CreateLeopardProfileDto leopardProfile)
        {
            var foundType = await _typeService.GetById(leopardProfile.LeopardTypeId);
            if(foundType == null)
            {
                return BadRequest(ErrorCodeModel.Invalid());
            }

            var newLeopardProfile = new LeopardProfile()
            {
                LeopardTypeId = leopardProfile.LeopardTypeId,
                LeopardName = leopardProfile.LeopardName,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate,
            };
            await _service.Create(newLeopardProfile);

            return CreatedAtAction("GetLeopardProfile", new { id = newLeopardProfile.LeopardProfileId }, leopardProfile);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> DeleteLeopardProfile(int id)
        {
            var leopardProfile = await _service.GetById(id);
            if (leopardProfile == null)
            {
                return NotFound(ErrorCodeModel.NotFound());
            }

            await _service.Delete(id);

            return NoContent();
        }

        private async Task<bool> LeopardProfileExists(int id)
        {
            var found = await _service.GetById(id);
            return found != null ? true : false;
        }
    }
}
