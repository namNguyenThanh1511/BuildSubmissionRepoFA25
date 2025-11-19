using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Query;
using System.Text.RegularExpressions;
using DAL.Model;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PRN231_SU25_SE170077.api.Controllers
{
    [Route("api/LeopardProfile")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        private readonly LeopardService _leopardService;

        public LeopardController(LeopardService leopardService)
        {
            _leopardService = leopardService;
        }

        [Authorize(Roles = "5,6,7,4")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _leopardService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "5,6,7,4")]
        public IActionResult GetById(int id)
        {
            var entity = _leopardService.GetById(id);
            if (entity == null)
            {
                return NotFound(new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            }

            return Ok(entity);
        }

        [Authorize(Roles = "5,6")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(model.LeopardName))
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            if (model.Weight <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            _leopardService.Create(model);

            return StatusCode(201);
        }

        [Authorize(Roles = "5,6")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errorCode = "HB40001",
                    message = "Missing/invalid input"
                });
            }

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(model.LeopardName))
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            if (model.Weight <= 0)
                return BadRequest(new { errorCode = "HB40001", message = "Missing/invalid input" });

            var existing = _leopardService.Update(id, model);
            if (existing == false)
            {
                return NotFound(new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            }

            return StatusCode(200);
        }


        [Authorize(Roles = "5,6")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var check = _leopardService.Delete(id);
            if (check == false)
            {
                return NotFound(new
                {
                    errorCode = "HB40401",
                    message = "Resource not found"
                });
            }

            return StatusCode(200);
        }

        [Authorize(Roles = "5,6,7,4")]
        [HttpGet("search")]
        public IActionResult Search([FromServices] ODataQueryOptions<ListAllModel> odataOptions, [FromQuery] string? name, [FromQuery] double? weight)
        {
            var query = _leopardService.SearchWithProjection(name, weight);

            var settings = new ODataValidationSettings();
            odataOptions.Validate(settings);

            var results = (IQueryable<ListAllModel>)odataOptions.ApplyTo(query);

            var filtered = results.ToList();

            return Ok(filtered);
        }
    }
}

