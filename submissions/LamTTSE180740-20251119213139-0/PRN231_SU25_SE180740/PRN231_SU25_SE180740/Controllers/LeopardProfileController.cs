using BLL.DTOs;
using BLL.Responses;
using BLL.Services;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace PRN231_SU25_SE180740.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeopardProfileController : ODataController
    {
        private readonly LeopardProfileService _service;
        public LeopardProfileController(LeopardProfileService handbagService)
        {
            _service = handbagService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var leopards = await _service.GetAllProfiles();
                return Ok(leopards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal Server Error"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var profile = await _service.GetProfileByIdAsync(id);
                if (profile == null)
                    return NotFound(new ErrorResponse("HB40401", "Resource not found"));

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal Server Error"));
            }
        }

        [HttpPost]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Create([FromBody] PostProfileDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
                }

                var createdProfile = await _service.CreateProfileAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.LeopardProfileId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal server error"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfile profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
                }

                if (id != profile.LeopardProfileId)
                    return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));

                var updatedProfile = await _service.UpdateProfileAsync(id, profile);
                return Ok(updatedProfile);
            }
            catch (ApiException ex)
            {
                return NotFound(new ErrorResponse(ex.ErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal server error"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteProfileAsync(id);
                if (!result)
                    return NotFound(new ErrorResponse("HB40401", "Resource not found"));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal server error"));
            }
        }

    }
}
