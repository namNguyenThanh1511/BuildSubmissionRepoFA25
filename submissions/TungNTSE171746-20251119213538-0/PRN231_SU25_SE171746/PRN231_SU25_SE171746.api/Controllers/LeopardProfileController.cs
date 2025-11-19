using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE171746.api.DTOs;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE171746.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;
        private readonly LeopardTypeService _typeService;

        public LeopardProfileController(ILeopardProfileService service, LeopardTypeService typeService)
        {
            _service = service;
            _typeService = typeService;
        }
        [HttpGet]
        [Authorize(Roles = "7,4,5,6")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            var rs = items.Select(x =>  new LeopardProfileDTO()
            {
                LeopardType = new LeopardTypeDTO()
                {
                    Description = x.LeopardType.Description,
                    LeopardTypeId = x.LeopardTypeId,
                    LeopardTypeName = x.LeopardName,
                    Origin = x.LeopardType.Origin,
                },
                LeopardName = x.LeopardName,
                CareNeeds = x.CareNeeds,
                Characteristics = x.Characteristics,
                ModifiedDate = x.ModifiedDate,
                Weight = x.Weight,
                LeopardProfileId = x.LeopardProfileId
            }).ToList();

            return Ok(rs);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "7,4,5,6")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
            var rs = new LeopardProfileDTO()
            {
                LeopardType = new LeopardTypeDTO()
                {
                    Description = item.LeopardType.Description,
                    LeopardTypeId = item.LeopardTypeId,
                    LeopardTypeName = item.LeopardName,
                    Origin = item.LeopardType.Origin,
                },
                LeopardName = item.LeopardName,
                CareNeeds = item.CareNeeds,
                Characteristics = item.Characteristics,
                ModifiedDate = item.ModifiedDate,
                Weight = item.Weight,
                LeopardProfileId = item.LeopardProfileId

            };

            return Ok(rs);
        }

        [HttpPost]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Create([FromBody] CreateDTO createDTO)
        {
            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

            if (string.IsNullOrWhiteSpace(createDTO.LeopardName) || !regex.IsMatch(createDTO.LeopardName))
            {
                return BadRequest(new { errorCode = "HB40002", message = "LeopardName is invalid" });
            }

            if (!(createDTO.Weight > 15))
            {
                return BadRequest(new { errorCode = "HB40003", message = "Weight must be greater than 15" });
            }


            var existedId = await _typeService.GetByIdAsync(createDTO.LeopardTypeId);
            if (existedId == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "LeopardTypeId is not existed" });
            }


            var leopardProfile = new LeopardProfile()
            {
                CareNeeds = createDTO.CareNeeds,
                Characteristics = createDTO.Characteristics,
                LeopardName = createDTO.LeopardName,
                LeopardTypeId = createDTO.LeopardTypeId,
                ModifiedDate = createDTO.ModifiedDate,
                Weight = createDTO.Weight
            };

            var result = await _service.CreateAsync(leopardProfile);
            if (result > 0)
            {

                var createdItem = await _service.GetByIdAsync(leopardProfile.LeopardProfileId);
                return CreatedAtAction(nameof(GetById), new { id = leopardProfile.LeopardProfileId }, createdItem);
            }
            return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "5,6")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateDTO updateDTO)
        {
            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

            if (string.IsNullOrWhiteSpace(updateDTO.LeopardName) || !regex.IsMatch(updateDTO.LeopardName))
            {
                return BadRequest(new { errorCode = "HB40002", message = "LeopardName is invalid" });
            }

            if (!(updateDTO.Weight > 15))
            {
                return BadRequest(new { errorCode = "HB40003", message = "Weight must be greater than 15" });
            }
            var existedId = await _typeService.GetByIdAsync(updateDTO.LeopardTypeId);
            if (existedId == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "LeopardTypeId is not existed" });
            }



            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
          

            existing.LeopardTypeId = updateDTO.LeopardTypeId;
            existing.LeopardType = null;
            existing.Weight = updateDTO.Weight;
            existing.Characteristics = updateDTO.Characteristics;
            existing.CareNeeds = updateDTO.CareNeeds;
            existing.ModifiedDate = updateDTO.ModifiedDate;


            var result = await _service.UpdateAsync(existing);
            if (result > 0)
            {
                var updated = await _service.GetByIdAsync(id);
                return Ok(updated);
            }
            return StatusCode(500, new { errorCode = "HB50001", message = "Internal server error" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }
            return Ok(new { message = "Item deleted successfully." });
        }


        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery]string? LeopardName, [FromQuery] double? Weightl)
        {
            var LeopardProfiles = await _service.SearchAsync(LeopardName, Weightl);
            var rs = LeopardProfiles.Select(x => new LeopardProfileDTO()
            {
                LeopardType = new LeopardTypeDTO()
                {
                    Description = x.LeopardType.Description,
                    LeopardTypeId = x.LeopardTypeId,
                    LeopardTypeName = x.LeopardName,
                    Origin = x.LeopardType.Origin,
                },
                LeopardName = x.LeopardName,
                CareNeeds = x.CareNeeds,
                Characteristics = x.Characteristics,
                ModifiedDate = x.ModifiedDate,
                Weight = x.Weight,
                LeopardProfileId = x.LeopardProfileId

            });

            return Ok(rs);
        }
       
    }
}
