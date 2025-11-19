using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repository.Implements;
using Repository.Interfaces;
using Repository.Requests;
using Repository.Response;
using System.Security.Claims;

namespace PRN232_SU25_SE170587.api.Controllers
{
    [Route("api/[controller]")]
    public class LeopardProfileController : Controller
    {
        private readonly IProfileRepository _profileRepository;

        public LeopardProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        [Authorize(Roles = "4,5,6,7")]
        public async Task<IActionResult> GetAll()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var result = await _profileRepository.GetAllAsync();
            return result.Match(
                (error, code) => StatusCode(code, error),
                (data, code) => StatusCode(code, data)
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "4,5,6,7")]
        public async Task<IActionResult> GetOne(int id)
        {
            var result = await _profileRepository.GetOneAsync(id);
            return result.Match(
                (error, code) => StatusCode(code, error),
                (data, code) => StatusCode(code, data)
            );
        }

        [HttpPost()]
        [Authorize(Roles = "5, 6")]
        public async Task<IActionResult> Create([FromBody] ProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
            }

            var result = await _profileRepository.CreateAsync(request);
            return result.Match(
                (error, code) => StatusCode(code, error),
                (data, code) => StatusCode(code, data)
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "5, 6")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
            }

            var result = await _profileRepository.UpdateAsync(id, request);
            return result.Match(
                (error, code) => StatusCode(code, error),
                (data, code) => StatusCode(code, data)
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "5, 6")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _profileRepository.DeleteAsync(id);
            return result.Match(
                (error, code) => StatusCode(code, error),
                (data, code) => StatusCode(code, data)
            );
        }

        [HttpGet("search")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> Search(string? LeopardName, double? Weight)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var result = await _profileRepository.SearchAsync(LeopardName, Weight);
            return result.Match(
                (error, code) => StatusCode(code, error),
                (data, code) => StatusCode(code, data)
            );
        }
    }
}
