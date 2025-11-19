using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PRN231_SU25_SE172332.api.Controllers
{
	[Route("api/LeopardType")]
	[ApiController]
	public class LeopardTypeController : ControllerBase
	{
		private readonly ILeopardTypeService _service;

		public LeopardTypeController(ILeopardTypeService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _service.GetAll();

			if (result == null || !result.Any())
			{
				return BadRequest(new
				{
					errorCode = "HB40001",
					message = "No Brands found"
				});
			}

			return Ok(result);
		}
	}
}
