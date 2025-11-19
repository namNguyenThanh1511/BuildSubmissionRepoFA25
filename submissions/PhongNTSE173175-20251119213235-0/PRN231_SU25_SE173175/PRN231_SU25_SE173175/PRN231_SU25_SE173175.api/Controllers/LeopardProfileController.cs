using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE173175.Repository.Base;
using PRN231_SU25_SE173175.Service.DTOs;
using PRN231_SU25_SE173175.Service.Interfaces;

namespace PRN231_SU25_SE173175.api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LeopardProfileController : ControllerBase
	{
		private readonly ILeopardProfileService _leopardService;

		public LeopardProfileController(ILeopardProfileService leopardService)
		{
			_leopardService = leopardService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IEnumerable<LeopardProfileResponse>> GetAll()
		{
			return await _leopardService.GetAllAsync();
		}
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetById(int id)
		{
			var handbag = await _leopardService.GetLeopardByIdAsync(id);
			return Ok(handbag);
		}

		[HttpPost]
		[Authorize(Roles = "5, 6")]
		public async Task<IActionResult> Create([FromBody] LeopardProfileRequest leodto)
		{

			if (leodto == null)
			{
				throw new BaseException(StatusCodes.Status400BadRequest, "Handbag data is required.");
			}
			var leopard = await _leopardService.CreateLeopardAsync(leodto);

			var response = await _leopardService.GetLeopardByIdAsync(leopard.LeopardProfileId);
			return CreatedAtAction(nameof(GetById), new { id = leopard.LeopardProfileId }, response);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "5, 6")]
		public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileUpdateRequest request)
		{

			await _leopardService.UpdateLeopardAsync(id, request);
			var handbag = await _leopardService.GetLeopardByIdAsync(id);
			return Ok(handbag);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "5, 6")]
		public async Task<IActionResult> Delete(int id)
		{
			await _leopardService.DeleteLeopardAsync(id);
			return Ok();
		}

		[HttpGet("search")]
		[EnableQuery]
		[Authorize]
		public async Task<ActionResult<IQueryable<LeopardProfileResponse>>> Search([FromQuery] LeopardSearchRequest request)
		{

			var results = await _leopardService.SearchLeopardsQueryableAsync(request.LeopardName, request.Weight);
			return Ok(results);
		}
	}
}
