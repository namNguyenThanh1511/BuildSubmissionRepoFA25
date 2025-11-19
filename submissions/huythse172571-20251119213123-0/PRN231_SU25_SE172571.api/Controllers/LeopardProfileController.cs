using BOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace PRN231_SU25_SE172571.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeopardProfileController : ODataController
    {
        private readonly ILeopardProfileService _leopardProfileService;

        public LeopardProfileController(ILeopardProfileService leopardProfileService)
        {
            _leopardProfileService = leopardProfileService;
        }


        [Authorize(Policy = "Full")]
        [HttpGet]
        public async Task<IActionResult> GetLeopardProfiles()
        {
            try
            {
                var leopads = await _leopardProfileService.GetLeopardProfiles();
                return Ok(leopads);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "HB50001",
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

      

        [Authorize(Policy = "Full")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeopardProfileById(int id)
        {
            try
            {
                var leopad = await _leopardProfileService.GetLeopardProfile(id);
                if (leopad == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorCode = "HB40401",
                        Message = $"Handbag with id {id} not found"
                    });
                }

                return Ok(leopad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "HB50001",
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [Authorize(Policy = "ReadOnly")]
        [HttpPost]
        public async Task<IActionResult> AddLeopardProfile([FromBody] LeopardProfileCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage ?? "Invalid input";
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "HB40001",
                    Message = firstError
                });
            }

            try
            {
                var leopad = new LeopardProfile
                {
                    LeopardTypeId = dto.LeopardTypeId,
                    LeopardName = dto.LeopardName,
                    Weight = dto.Weight,
                    Characteristics = dto.Characteristics,
                    CareNeeds = dto.CareNeeds,
                    ModifiedDate = dto.ModifiedDate,                 
                };

                
       

        var created = await _leopardProfileService.AddLeopardProfile(leopad);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "HB50001",
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [Authorize(Policy = "ReadOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeopardProfile(int id, [FromBody] LeopardProfile leopardProfile)
        {
            try
            {
                leopardProfile.LeopardProfileId = id;
                var updatedleopardProfile = await _leopardProfileService.UpdateLeopardProfile(leopardProfile);
                return Ok(updatedleopardProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "HB50001",
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [Authorize(Policy = "ReadOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeopardProfile(int id)
        {
            try
            {
                var deletedleopardProfile = await _leopardProfileService.DeleteLeopardProfile(id);
                return Ok(deletedleopardProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "HB50001",
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }       
    }
}
