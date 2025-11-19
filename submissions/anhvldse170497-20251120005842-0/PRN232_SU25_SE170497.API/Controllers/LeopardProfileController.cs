using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN232_SU25_SE170497.API.Extensions;
using PRN232_SU25_SE170497.DAL.DTOs;

namespace PRN231.API.Controllers
{
    [Route("api/LeopardProfiles")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "4, 5, 6, 7")]
        public async Task<IActionResult> GetAllLeopardProfiles()
        {
            var result = await _service.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "4, 5, 6, 7")]
        public async Task<IActionResult> GetLeopardProfileById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "5, 6")]
        public async Task<IActionResult> CreateLeopardProfile(LeopardProfileDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "5, 6")]
        public async Task<IActionResult> UpdateLeopardProfile(int id, LeopardProfileDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "5, 6")]
        public async Task<IActionResult> DeleteLeopardProfile(int id)
        {
            var result = await _service.DeleteByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("search")]
        [Authorize(Roles = "4, 5, 6, 7, ")]
        [EnableQuery]
        public async Task<IActionResult> Search()
        {
            var response = await _service.ListAllAsync();
            return Ok(response);
        }

    }
}
