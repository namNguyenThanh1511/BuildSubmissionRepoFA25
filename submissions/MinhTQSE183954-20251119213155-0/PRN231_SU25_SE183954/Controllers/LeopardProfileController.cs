using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Impl;
using WebAPI.Controllers;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("api/LeopardProfile")]
	public class LeopardProfileController : BaseController
	{
		private readonly LeopardProfileService _service;

		public LeopardProfileController(LeopardProfileService service) 
		{ 
			_service = service;
		}
		[Authorize(Roles = "5,6,7,4")]
		[HttpGet]
		public async Task<IActionResult> GetLeopard()
		{
			try
			{
				return Ok(await _service.GetAll());
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}
		[Authorize(Roles = "5,6,7,4")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetHandbagById([FromRoute] int id)
		{
			try
			{
				return Ok(await _service.GetById(id));
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}
		[Authorize(Roles = "5,6")]
		[HttpPost]
		public async Task<IActionResult> AddHandbag([FromBody] CreateLeopardReq request)
		{
			try
			{
				await _service.AddLeopard(request);
				return StatusCode(201, new { message = "Leopard added." });
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}
		[Authorize(Roles = "5,6")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateHandbag([FromRoute] int id, [FromBody] UpdateLeopardReq request)
		{
			try
			{
				await _service.UpdateLeopard(id, request);
				return Ok(new { messgae = "Leopard updated" });
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}
		[Authorize(Roles = "5,6")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLeopard([FromRoute] int id)
		{
			try
			{
				await _service.DeleteLeopard(id);
				return Ok(new { messgae = "Leopard deleted" });
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}
	}
}
