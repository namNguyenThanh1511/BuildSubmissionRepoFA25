using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE184736.api.ViewModels;
using Repositories.Models;
using Services;

namespace PRN231_SU25_SE184736.api.Controllers
{
    public class LeopardProfileController : ODataController
    {
        private readonly LeopardProfileService _service;

        public LeopardProfileController(LeopardProfileService service)
        {
            _service = service;
        }

         //GET: api/LeopardProfiles
        [EnableQuery]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> Get()
        {
            return await _service.GetAll();
        }

        // GET: api/LeopardProfiles/5
        [HttpGet("api/{id}")]
        [Authorize]
        public async Task<ActionResult<LeopardProfile>> GetById(int id)
        {
            var leopardProfile = await _service.GetById(id);

            if (leopardProfile == null)
            {
                return NotFound(ErrorCodeModel.NotFound());
            }

            return leopardProfile;
        }

        // PUT: api/LeopardProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/[controller]/{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] LeopardProfileView leopardProfile)
        {
            if (!ModelState.IsValid)
            {
                var output = ErrorCodeModel.InvalidInput();
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                output.Message = string.Join("; ", errors);

                return BadRequest(output);
            }


            if (id != leopardProfile.LeopardProfileId)
            {
                return BadRequest(ErrorCodeModel.InvalidInput());
            }

            try
            {
                var exist = await _service.GetById(id);
                if (exist == null)
                {
                    return NotFound(ErrorCodeModel.NotFound());
                }

                exist.LeopardName = leopardProfile.LeopardName;
                exist.CareNeeds = leopardProfile.CareNeeds;
                exist.LeopardTypeId = leopardProfile.LeopardTypeId;
                exist.Characteristics = leopardProfile.Characteristics;
                exist.ModifiedDate = leopardProfile.ModifiedDate;
                exist.Weight = leopardProfile.Weight;

                await _service.Update(exist);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorCodeModel.ServerError());
            }

            return NoContent();
        }

        [HttpPost("api/[controller]")]
        [Authorize(Roles = "5 6")]
        public async Task<ActionResult<LeopardProfile>> Create([FromBody] LeopardProfileView req)
        {
            if (!ModelState.IsValid)
            {
                var output = ErrorCodeModel.InvalidInput();
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                output.Message = string.Join("; ", errors);

                return BadRequest(output);
            }

            var addProfile = new LeopardProfile()
            {
                LeopardProfileId = req.LeopardProfileId,
                LeopardTypeId = req.LeopardTypeId,
                LeopardName = req.LeopardName,
                Weight = req.Weight,
                Characteristics = req.Characteristics,
                CareNeeds = req.CareNeeds,
                ModifiedDate = req.ModifiedDate,
            };

            try
            {
                await _service.Create(addProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorCodeModel.ServerError());
            }

            return Ok(addProfile);
        }

        // DELETE: api/LeopardProfiles/5
        [HttpDelete("api/[controller]/{id}")]
        [Authorize(Roles = "5 6")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var leopardProfile = await _service.GetById(id);
            if (leopardProfile == null)
            {
                return NotFound(ErrorCodeModel.NotFound());
            }

            await _service.Delete(id);

            return NoContent();
        }
    }
}
