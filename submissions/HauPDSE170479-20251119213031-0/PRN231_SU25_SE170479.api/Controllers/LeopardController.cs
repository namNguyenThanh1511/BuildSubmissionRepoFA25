using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE170479.api.Extensions;
using PRN231_SU25_SE170479.BLL.Services;
using PRN231_SU25_SE170479.DAL.DTOs;

namespace PRN231_SU25_SE170479.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        private readonly ILeopardService _service;

        public LeopardController(ILeopardService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "1, 2, 3, 4")]
        public async Task<IActionResult> GetAllLeopard()
        {
            var result = await _service.GetAllAsync();
            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "1, 2, 3, 4")]
        public async Task<IActionResult> GetLeopardById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> CreateLeopard(LeopardDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> UpdateLeopard(int id, LeopardDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> DeleteLeopard(int id)
        {
            var result = await _service.DeleteByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("search")]
        [Authorize(Roles = "1, 2, 3, 4")]
        [EnableQuery]
        public async Task<IActionResult> Search()
        {
            var response = await _service.ListAllAsync();
            return Ok(response);
        }

    }
}
