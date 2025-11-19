using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.DTOs;
using Repository.Models;
using Service.Interface;

namespace PRN231_SU25_SE173566.api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class LeopardProfileController : ControllerBase
	{

		private readonly ILeopardService _leopardService;

		public LeopardProfileController(ILeopardService leopardService)
		{
			_leopardService = leopardService;
		}


	
		[HttpGet]
		public async Task<IEnumerable<LeopardProfile>> Get()
		{
			return await _leopardService.GetAllAsync();
		}

		
		[HttpGet("({id})")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			var result = await _leopardService.GetByIdAsync(id);
			if (result == null)
				return NotFound(new ErrorResponse { ErrorCode = ErrorCodes.NotFound, Message = "Resource not found" });
			return Ok(result);
		}

		
		[HttpPost]
		[Authorize(Policy = "AdminOrStaff")]
		public async Task<IActionResult> Post([FromBody] LeopardProfileCreateDTO cosmetic)
		{
			if (!ModelState.IsValid)
				return BadRequest(new ErrorResponse { ErrorCode = ErrorCodes.BadRequest, Message = "Invalid input" });

			await _leopardService.AddAsync(cosmetic);
			return Ok(cosmetic);
		}

		
		[HttpPut("({id})")]
		[Authorize(Policy = "AdminOrStaff")]
		public async Task<IActionResult> Put([FromRoute] int id, [FromBody] LeopardProfile update)
		{
			if (id != update.LeopardProfileId)
				return BadRequest(new ErrorResponse { ErrorCode = ErrorCodes.BadRequest, Message = "CosmeticId mismatch" });

			var existing = await _leopardService.GetByIdAsync(id);
			if (existing == null)
				return NotFound(new ErrorResponse { ErrorCode = ErrorCodes.NotFound, Message = "Resource not found" });

			await _leopardService.UpdateAsync(update);
			return Ok(update);
		}

	
		[HttpDelete("({id})")]
		[Authorize(Policy = "AdminOrStaff")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var existing = await _leopardService.GetByIdAsync(id);
			if (existing == null)
				return NotFound(new ErrorResponse { ErrorCode = ErrorCodes.NotFound, Message = "Resource not found" });

			await _leopardService.DeleteAsync(id);
			return NoContent();
		}
	}
}
