using BLL;
using BLL.ModelView;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE173524.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        public readonly ILeopardProfileBL _leopardProfileBL;

        public LeopardProfileController(ILeopardProfileBL leopardProfileBL)
        {
            _leopardProfileBL = leopardProfileBL;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = "ReadAccessOnly")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetAlls()
        {
            var handbags = await _leopardProfileBL.GetAll();
            return Ok(handbags);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "ReadAccessOnly")]
        public async Task<ActionResult<LeopardProfile>> GetHandbagById(int id)
        {
            var handbag = await _leopardProfileBL.GetById(id);
            if (handbag == null)
                throw new KeyNotFoundException("HB40401: Handbag with ID not found.");

            return Ok(handbag);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrModerator")]
        public async Task<ActionResult<LeopardProfile>> Create([FromBody] LeopardProfileRequestDTO dto)
        {
            if (dto == null)
                throw new ValidationException("HB40001: Handbag data is null.");
                //throw new ValidationException();

          

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(dto.LeopardName))
                throw new ValidationException("HB40001: Name is invalid format");
                //throw new ValidationException();

            if (dto.Weight <= 15)
                throw new ValidationException("HB40001: Weight must be greater than 0");
               // throw new ValidationException();


            

            

            var leopard = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                LeopardTypeId = dto.LeopardTypeId,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
            };

            var created = await _leopardProfileBL.Create(leopard);
            return CreatedAtAction(nameof(GetHandbagById), new { id = created.LeopardProfileId }, created);
        }


        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOrModerator")]
        public async Task<ActionResult<LeopardProfile>> Update(int id, [FromBody] LeopardProfileRequestDTO1 dto)
        {
            if (dto == null)
                throw new ValidationException("HB40001: Leopard data is null.");
            //throw new ValidationException();

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(dto.LeopardName))
                throw new ValidationException("HB40001: Name is invalid format");
            //throw new ValidationException();

            if (dto.Weight <= 15)
                throw new ValidationException("HB40001: Weight must be greater than 0");
                //throw new ValidationException();


            var leopard = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                LeopardTypeId = dto.LeopardTypeId,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
            };

            var updated = await _leopardProfileBL.Update(id, leopard);
            if (updated == null)
                throw new KeyNotFoundException("HB40401: Handbag with ID not found.");

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOrModerator")]
        public async Task<ActionResult<LeopardProfile>> Delete(int id)
        {
            var deletedHandbag = await _leopardProfileBL.Delete(id);
            if (deletedHandbag == null)
                throw new KeyNotFoundException("HB40401: Handbag with ID not found.");

            return Ok(deletedHandbag);
        }
    }
}
