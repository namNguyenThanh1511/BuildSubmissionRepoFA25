using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;

namespace WebAPI.Controllers;

public abstract class BaseController : ControllerBase
{
	protected IActionResult HandleException(Exception ex)
	{
		switch (ex)
		{
			case KeyNotFoundException _:
				return NotFound(ErrorRes.NotFound("Resource not found"));
			case NullReferenceException _:
				return NotFound(ErrorRes.NotFound("Resource not found"));
			case UnauthorizedAccessException _:
				return Unauthorized(ErrorRes.Unauthorized("Permission denied"));
			default:
				return StatusCode(500, ErrorRes.InternalErrorServer(ex.Message));
		}
	}
}