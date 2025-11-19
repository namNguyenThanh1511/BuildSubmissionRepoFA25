using System.Text.RegularExpressions;
using BusinessLogicLayer.Services;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Query;

namespace PRN232_SU25_SE170076_DuongThanhThoai.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeoPardController : Controller
    {
        private readonly LeoPardService _leoPardService;

        public LeoPardController(LeoPardService leoPardService)
        {
            _leoPardService = leoPardService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllHandBag()
        {
            var list = _leoPardService.GetAllLeoPard();
            return Ok(list);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetLeoPardId(int id)
        {
            var leopard = _leoPardService.GetLeoPardId(id);
            if (leopard == null)
            {
                return NotFound(new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            }
            return Ok(leopard);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateLeopard([FromBody] CreateLeoPardModel model)
        {

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(model.LeopardName))
                return BadRequest(new { errorCode = "HB40001", message = "ModelName does not match required format" });
            try
            {
                _leoPardService.CreateLeopard(model);
                return StatusCode(201, model);
   
            }
            catch (ArgumentException ex)
            {
                return StatusCode(403, new { errorCode = "HB40302", message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal sever error" });
            }

        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteLeoPard(int id)
        {
            var existing = _leoPardService.GetLeoPardId(id);
            if (existing == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }

            try
            {
                _leoPardService.DeleteLeoPard(id);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(401, new { errorCode = "HB40101", message = "Token missing/invalid" });
            }
            catch (ArgumentException)
            {
                return StatusCode(403, new { errorCode = "HB40301", message = "Permission denied" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal sever error" });
            }
        }

        //API PUT
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateLeoPard(int id, [FromBody] CreateLeoPardModel model)
        {
            //400
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });
            }

            var existing = _leoPardService.GetLeoPardId(id);
            if (existing == null)
            {
                return NotFound(new { errorCode = "HB40401", message = "Resource not found" });
            }

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!string.IsNullOrEmpty(model.LeopardName) && !regex.IsMatch(model.LeopardName))
            {
                return BadRequest(new { errorCode = "HB40001", message = "ModelName does not match required format" });
            }

            if (model.Weight > 15)
            {
                return BadRequest(new { errorCode = "HB40001", message = "Price and stock must be greater than 0" });
            }

            var allHandbags = _leoPardService.GetAllLeoPard();

            try
            {
                _leoPardService.UpdateLeoPard(id, model);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(401, new { errorCode = "HB40101", message = "Token missing/invalid" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { errorCode = "HB50001", message = "Internal sever error" });
            }

        }

        [Authorize]
        [HttpGet("search")]
        public IActionResult SearchShoe([FromServices] ODataQueryOptions<LeoPardModel> odataOptions, [FromQuery] string? leopardname, [FromQuery] string? weight)
        {
            var query = _leoPardService.SearchWithProjection(leopardname, weight);

            var settings = new ODataValidationSettings();
            odataOptions.Validate(settings);

            var results = (IQueryable<LeoPardModel>)odataOptions.ApplyTo(query);

            var filtered = results.ToList(); 

            var grouped = filtered
                .GroupBy(h => h.LeopardTypeName)
                .Select(g => new GroupedHandbagDto
                {
                    LeoPardTypeName = g.Key,
                    LeoPards = g.ToList()
                });

            return Ok(grouped);
        }

    }
}
