using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE172332.api.DTOs;
using Repositories.Models;
using Services;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE172332.api.Controllers
{
	[Route("api/LeopardProfile")]
	[ApiController]
	public class LeopardProfileController : ControllerBase
	{
		private readonly ILeopardProfileService _service;

		public LeopardProfileController(ILeopardProfileService service)
		{
			_service = service;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAll()
		{
			var list = await _service.GetAll();
			return Ok(list);
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetById(int id)
		{
			var item = await _service.GetById(id);
			if (item == null)
				return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

			return Ok(item);
		}

		[HttpGet("search")]
		[Authorize]
		public async Task<IActionResult> Search([FromQuery] string? leopardName, [FromQuery] double? weight)
		{
			var results = await _service.Search(leopardName, weight);
			return Ok(results);
		}

		[HttpPost]
		[Authorize(Roles = "administrator,moderator")]
		public async Task<IActionResult> Create([FromBody] CreateDTO dto)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(dto.LeopardName) ||
				!Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
					return BadRequest(new { errorCode = "HB40001", message = "LeopardName is invalid format and required" });

				if (dto.Weight == null || dto.Weight <= 15)
					return BadRequest(new { errorCode = "HB40001", message = "Weight is required and must be greater than 15" });

				var newId = await _service.GetNextId();

				var profile = new LeopardProfile
				{
					LeopardProfileId = 0,
					Weight = dto.Weight,
					LeopardName = dto.LeopardName,
					CareNeeds = dto.CareNeeds,
					Characteristics = dto.Characteristics,
					LeopardTypeId = dto.LeopardTypeId,
					ModifiedDate = DateTime.Now,
				};

				var result = await _service.Create(profile);

				var item = await _service.GetById(newId);
				if (item == null)
					return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

				return CreatedAtAction(nameof(GetById), new { id = item.LeopardProfileId }, item);
			}
			catch (Exception ex)
			{
				return StatusCode(
					500,
					new
					{
						errorCode = "HB50001",
						message = "Internal Server: " + ex.InnerException.Message
					});
			}
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "administrator,moderator")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateDto dto)
		{
			try
			{
				var existing = await _service.GetById(id);
				if (existing == null)
					return NotFound();

				if (dto.LeopardName != null)
				{
					var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
					if (!regex.IsMatch(dto.LeopardName))
						return BadRequest(new { errorCode = "HB40001", message = "Invalid LeopardName format" });

					existing.LeopardName = dto.LeopardName;
				}

				if (dto.CareNeeds != null)
					existing.CareNeeds = dto.CareNeeds;

				if (dto.Characteristics != null)
					existing.Characteristics = dto.Characteristics;

				if (dto.Weight.HasValue)
				{
					if (dto.Weight < -15)
						return BadRequest(new { errorCode = "HB40001", message = "Weight must be > 15" });
					existing.Weight = dto.Weight.GetValueOrDefault();
				}

				if (dto.LeopardTypeId.HasValue)
				{
					existing.LeopardTypeId = dto.LeopardTypeId.GetValueOrDefault();
				}

				await _service.Update(existing);

				var updated = await _service.GetById(id);
				return Ok(updated);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					errorCode = "HB50001",
					message = "Internal Server: " + ex.InnerException.Message
				});
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "administrator,moderator")]
		public async Task<IActionResult> Delete(int id)
		{
			var existing = await _service.GetById(id);
			if (existing == null)
				return NotFound(new { errorCode = "HB40401", message = "Resource not found" });

			await _service.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}
	}
}
