using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE170489.API.Extensions;
using PRN231_SU25_SE170489.BLL.Services;
using PRN231_SU25_SE170489.DAL.DTOs;

namespace PRN231_SU25_SE170489.API.Controllers
{
	[Route("api/leopardProfiles")]
	[ApiController]
	public class LeopardProfileController : ControllerBase
	{
		private readonly ILeopardProfileService _service;

		public LeopardProfileController(ILeopardProfileService service)
		{
			_service = service;
		}

		[HttpGet]
		[Authorize(Roles = "1, 2, 3, 4")]
		public async Task<IActionResult> GetAllLeopardProfiles()
		{
			var result = await _service.GetAllAsync();
			return result.ToActionResult();
		}

		[HttpGet("{id:int}")]
		[Authorize(Roles = "1, 2, 3, 4")]
		public async Task<IActionResult> GetLeopardProfileById(int id)
		{
			var result = await _service.GetByIdAsync(id);
			return result.ToActionResult();
		}

		[HttpPost]
		[Authorize(Roles = "1, 2")]
		public async Task<IActionResult> CreateLeopardProfile(LeopardProfileDTO dto)
		{
			var result = await _service.CreateAsync(dto);
			return result.ToActionResult();
		}

		[HttpPut("{id:int}")]
		[Authorize(Roles = "1, 2")]
		public async Task<IActionResult> UpdateLeopardProfile(int id, LeopardProfileDTO dto)
		{
			var result = await _service.UpdateAsync(id, dto);
			return result.ToActionResult();
		}

		[HttpDelete("{id:int}")]
		[Authorize(Roles = "1, 2")]
		public async Task<IActionResult> DeleteLeopardProfile(int id)
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
