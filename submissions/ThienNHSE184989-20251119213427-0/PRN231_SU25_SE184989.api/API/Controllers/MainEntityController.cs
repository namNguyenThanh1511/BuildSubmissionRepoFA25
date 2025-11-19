using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainEntityController : ControllerBase
    {
        private readonly IMainEntityService _mainEntityService;

        public MainEntityController(IMainEntityService mainEntityService)
        {
            _mainEntityService = mainEntityService;
        }

        [HttpGet("LeopardProfile")]
        [Authorize(Roles = "5,6,7,4")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mainEntityService.GetAllMainEntity();
            return Ok(list);
        }

        [HttpGet("LeopardProfile/{id}")]
        [Authorize(Roles = "5,6,7,4")]
        public async Task<IActionResult> GetMainEntityById(int id)
        {
            var result = await _mainEntityService.GetMainEntityById(id);
            return Ok(result);
        }

        [HttpPost("LeopardProfile")]
        [Authorize(Roles = "5,6")]  
        public async Task<IActionResult> CreateEntity([FromBody] CreateEntityRequest request)
        {
            // Validation
            if (string.IsNullOrEmpty(request.LeopardName))
                throw new ArgumentException("LeopardName is required");

            // Add regex validation if needed
            if (!System.Text.RegularExpressions.Regex.IsMatch(request.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                throw new ArgumentException("LeopardName format is invalid");

            if (request.Weight <= 15)
                throw new ArgumentException("Weight must be greater than 15");

            var entity = new Repository.Models.LeopardProfile
            {
                LeopardTypeId = request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                ModifiedDate = request.ModifiedDate
            };

            var created = await _mainEntityService.CreateMainEntity(entity);
            return StatusCode(201, created);
        }

        [HttpPut("LeopardProfile/{id}")]
        [Authorize(Roles = "5,6")] 
        public async Task<IActionResult> UpdateEntity(int id, [FromBody] CreateEntityRequest request)
        {
            // Same validation as POST
            if (string.IsNullOrEmpty(request.LeopardName))
                throw new ArgumentException("LeopardName is required");

            // Add regex validation if needed
            if (!System.Text.RegularExpressions.Regex.IsMatch(request.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                throw new ArgumentException("LeopardName format is invalid");

            if (request.Weight <= 15)
                throw new ArgumentException("Weight must be greater than 15");

            var entity = new Repository.Models.LeopardProfile
            {
                LeopardTypeId = request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds = request.CareNeeds,
                ModifiedDate = request.ModifiedDate
            };

            var updated = await _mainEntityService.UpdateMainEntity(id, entity);
            return Ok(updated);
        }

        [HttpDelete("LeopardProfile/{id}")]
        [Authorize(Roles = "5,6")] 
        public async Task<IActionResult> DeleteEntity(int id)
        {
            var deleted = await _mainEntityService.DeleteMainEntity(id);
            if (deleted)
                return Ok(new { message = "Entity deleted successfully" });
            else
                throw new KeyNotFoundException($"Entity with ID {id} not found");
        }

        [HttpGet("LeopardProfile/search")]
        [Authorize(Roles = "5,6,7,4")]  // All roles with token
        public async Task<IActionResult> SearchEntities([FromQuery] string? leopardname, [FromQuery] double? weight)
        {
            var results = await _mainEntityService.SearchMainEntity(leopardname, weight);
            return Ok(results);
        }
    }

    public class CreateEntityRequest
    {
        public int LeopardTypeId { get; set; }
        public string LeopardName { get; set; }
        public double Weight { get; set; }
        public string Characteristics { get; set; }
        public string CareNeeds { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

}
