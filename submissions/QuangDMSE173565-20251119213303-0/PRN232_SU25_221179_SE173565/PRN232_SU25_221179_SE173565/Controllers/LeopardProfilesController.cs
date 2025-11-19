using BLL;
using BLL.ModelView;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PRN232_SU25_221179_SE173565.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardProfilesController : ControllerBase
    {

        public readonly ILeopardProfileBL _service;

        public LeopardProfilesController(ILeopardProfileBL service)
        {
            _service = service;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = "FullFunction")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetAllObjects()
        {
            var objects = await _service.GetAll();
            return Ok(objects);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "FullFunction")]
        public async Task<ActionResult<LeopardProfile>> GetObjectById(int id)
        {
            var objects = await _service.GetById(id);
            if (objects == null)
                throw new KeyNotFoundException("HB40401: LeopardProfile with ID not found.");

            return Ok(objects);
        }

        [HttpPost]
        [Authorize(Policy = "FullFunction")]
        public async Task<ActionResult<LeopardProfile>> CreateObject([FromBody] LeopardProfileDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Request body is null.");

                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9@#]*\s)*[A-Z0-9][a-zA-Z0-9@#]*$");
                if (!regex.IsMatch(dto.LeopardName))
                    return BadRequest("LeopardName is invalid format.");

                if (dto.Weight < 15)
                    return BadRequest("Weight must be at least 15.");

                var item = new LeopardProfile
                {
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
                LeopardTypeId = dto.LeopardTypeId,
            };

            var createdObject = await _service.Create(item);
            return CreatedAtAction(nameof(GetObjectById), new { id = createdObject.LeopardProfileId }, createdObject);
        }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (ValidationException ex)
    {
        return BadRequest(ex.Message);
}
    catch (Exception ex)
    {
        // Log lỗi nếu cần
        return StatusCode(500, "An unexpected error occurred.");
    }
}


        [HttpPut("{id}")]
        [Authorize(Policy = "FullFunction")]
        public async Task<ActionResult<LeopardProfile>> UpdateObject(int id, [FromBody] LeopardProfileDTO1 dto)
        {
            if (dto == null)
                // throw new ValidationException("HB40001: LeopardProfile data is invalid or ID mismatch.");
                throw new ValidationException();

            //if (string.IsNullOrWhiteSpace(dto.LeopardName))
            //    //throw new ValidationException("HB40001: modelName is required");
            //    throw new ValidationException();

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(dto.LeopardName))
                //throw new ValidationException("HB40001: modelName is invalid format");
                throw new ValidationException();

            var item = new LeopardProfile
            {
                LeopardProfileId = id,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = DateTime.Now,             
                LeopardTypeId = dto.LeopardTypeId,
            };

            var updatedObject = await _service.Update(item);
            if (updatedObject == null)
                throw new KeyNotFoundException("HB40401: LeopardProfile with ID not found.");

            return Ok(updatedObject);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "FullFunction")]
        public async Task<ActionResult<LeopardProfile>> DeleteObject(int id)
        {
            var deletedObject = await _service.Delete(id);
            if (deletedObject == null)
                throw new KeyNotFoundException("HB40401: LeopardProfile with ID not found.");

            return Ok(deletedObject);
        }
    }
}
