using BLL;
using BLL.ModelView;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Source_PRN3_1.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        // Business logic layer cho Handbag
        public readonly ILeopardBL leopardBL;

        // Constructor: Inject BL
        public LeopardController(ILeopardBL systemAccountService)
        {
            leopardBL = systemAccountService;
        }


        [HttpPost]
        [Authorize(Policy = "FullAccess")]
        public async Task<ActionResult<LeopardProfile>> Create([FromBody] LeopardRequestDTO dto)
        {
            if (dto == null)
                throw new ValidationException();
            if (string.IsNullOrWhiteSpace(dto.LeopardName))
                throw new ValidationException();
            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(dto.LeopardName))
                throw new ValidationException();
            if (string.IsNullOrWhiteSpace(dto.Characteristics))
                throw new ValidationException();
            if (dto.Weight >15)
                throw new ValidationException();
            if (string.IsNullOrWhiteSpace(dto.CareNeeds))
                throw new ValidationException();

            var entity = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                Weight = dto.Weight,
                LeopardTypeId = dto.LeopardTypeId,
                ModifiedDate = DateTime.Now
            };
            var result = await leopardBL.Create(entity);
            return CreatedAtAction(nameof(GetById), new { id = result.LeopardProfileId }, result);
        }


        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = "ReadAccessOnly")]
        public async Task<ActionResult<IEnumerable<LeopardProfile>>> GetAll()
        {
            var result = await leopardBL.GetAll();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "ReadAccessOnly")]
        public async Task<ActionResult<LeopardProfile>> GetById(int id)
        {
            var result = await leopardBL.GetById(id);
            if (result == null)
                throw new KeyNotFoundException("HB40401: Leopard with ID not found.");
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<ActionResult<LeopardProfile>> Update(int id, [FromBody] LeopardRequestDTO dto)
        {
            if (dto == null)
                throw new ValidationException();
            if (string.IsNullOrWhiteSpace(dto.LeopardName))
                throw new ValidationException();
            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(dto.LeopardName))
                throw new ValidationException();
            if (string.IsNullOrWhiteSpace(dto.Characteristics))
                throw new ValidationException();
            if (dto.Weight >15)
                throw new ValidationException();
            if (string.IsNullOrWhiteSpace(dto.CareNeeds))
                throw new ValidationException();


            var entity = new LeopardProfile
            {
                LeopardName = dto.LeopardName,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                Weight = dto.Weight,
                LeopardTypeId = dto.LeopardTypeId,
                ModifiedDate = DateTime.Now
            };

  
            var result = await leopardBL.Update(entity);
            if (result == null)
                throw new KeyNotFoundException("HB40401: Leopard with ID not found.");
            return Ok(result);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<ActionResult<LeopardProfile>> Delete(int id)
        {
            var result = await leopardBL.Delete(id);
            if (result == null)
                throw new KeyNotFoundException("HB40401: Leopard with ID not found.");
            return Ok(result);
        }
    }
}
