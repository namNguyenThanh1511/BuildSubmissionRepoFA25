using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Model;
using Services;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.OData.Query.Validator;
using System.Net;
using Helper;

namespace PRN231_SU25_SE170115.api.Controllers
{
    [ApiController]
    [Route("api/LeopardProfile")]
    [Authorize]
    public class LeopardProfilesController : ControllerBase
    {
        private readonly LeopardProfileService _service;

        public LeopardProfilesController(LeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult GetAll()
        {
            var items = _service.GetAlLeopardProfile();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult GeById(int id)
        {
            var items = _service.GetLeopardProfileId(id);
            if (items == null)
                return ErrorResultHelper.Create(HttpStatusCode.NotFound);

            return Ok(items);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Create([FromBody] CreateLeopardProfileModel model)
        {
            if (!ModelState.IsValid)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(model.LeopardName))
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            if (model.Weight <= 15)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var result = _service.CreateLeopardProfile(model);
            if (result== null)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            return StatusCode(201, new { message = "Created successfully", item = result });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Update(int id, [FromBody] CreateLeopardProfileModel model)
        {
            if (id <= 0)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var item = _service.GetLeopardProfileId(id);
            if (item == null)
                return ErrorResultHelper.Create(HttpStatusCode.NotFound);

            if (!ModelState.IsValid)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
            if (!regex.IsMatch(model.LeopardName))
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            if (model.Weight <= 15)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var result = _service.UpdateLeopardProfile(id, model);
            if (!result)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return ErrorResultHelper.Create(HttpStatusCode.BadRequest);

            var result = _service.DeleteLeopardProfile(id);
            if (!result)
                return ErrorResultHelper.Create(HttpStatusCode.NotFound);

            return Ok(new { message = "Deleted successfully" });
        }

        [Authorize(Roles = "administrator,moderator,developer,member")]
        [HttpGet("search")]
        public IActionResult SearchHandbags([FromServices] ODataQueryOptions<ListLeopardProfileModel> odataOptions, [FromQuery] string? leopardName, [FromQuery] double? weight)
        {
            var query = _service.SearchWithProjection(leopardName, weight);

            var settings = new ODataValidationSettings();

            try
            {
                odataOptions.Validate(settings);

                var results = (IQueryable<ListLeopardProfileModel>)odataOptions.ApplyTo(query);
                var filtered = results.ToList();

                return Ok(results);
            }
            catch (Exception)
            {
                return ErrorResultHelper.Create(HttpStatusCode.InternalServerError);
            }
        }
    }
}
