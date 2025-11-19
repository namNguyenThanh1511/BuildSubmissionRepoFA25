using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE184386.BLL.Services;
using PRN231_SU25_SE184386.DAL.ModelExtensions;

namespace PRN231_SU25_SE184386.API.Controllers
{
    [Route("api/LeopardProfiles")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LeopardProfileService _service;

        public LeopardProfileController(LeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetAllLeopardProfiles()
        {
            var response = await _service.GetAllLeopardProfilesAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetAllLeopardProfileById([FromRoute] int id)
        {
            var response = await _service.GetLeopardProfileByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrator,moderator")]
        public async Task<IActionResult> CreateLeopardProfile([FromBody] LeopardProfileDTO request)
        {
            var response = await _service.CreateLeopardProfileAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrator,moderator")]
        public async Task<IActionResult> UpdateLeopardProfile([FromRoute] int id, [FromBody] LeopardProfileDTO request)
        {
            var response = await _service.UpdateLeopardProfileAsync(id, request);
            return response.Success ?
                Ok(response) :
                response?.DetailError?.ErrorCode == "HB40401" ?
                    NotFound(response) :
                    BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrator,moderator")]
        public async Task<IActionResult> DeleteLeopardProfile([FromRoute] int id)
        {
            var response = await _service.DeleteLeopardProfileByIdAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("search")]
        [EnableQuery]
        public async Task<IActionResult> Search()
        {
            var response = await _service.ListAllAsync();
            return Ok(response);
        }
    }
}
