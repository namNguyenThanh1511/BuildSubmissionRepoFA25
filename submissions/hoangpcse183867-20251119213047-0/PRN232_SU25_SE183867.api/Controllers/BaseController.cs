using Microsoft.AspNetCore.Mvc;

namespace PRN232_SU25_SE183867.api.Controllers
{
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
                case ArgumentException _:
                    return BadRequest(ErrorRes.BadRequest(ex.Message));
                case UnauthorizedAccessException _:
                    return Unauthorized(ErrorRes.Unauthorized("Permission denied"));
                default:
                    return StatusCode(500, ErrorRes.InternalErrorServer(ex.Message));
            }
        }
    }
}
