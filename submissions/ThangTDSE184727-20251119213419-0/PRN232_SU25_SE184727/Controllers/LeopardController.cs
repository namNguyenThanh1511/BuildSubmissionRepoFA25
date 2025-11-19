using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN232_SU25_SE184727.Models;
using Repositories.Models;
using Services;
using Services.ViewModel;
using System.Threading.Tasks;

namespace PRN232_SU25_SE184727.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardController : ControllerBase
    {
        private readonly LeopardProfileService _leopardProfileService;
        public LeopardController(LeopardProfileService leopardProfileService)
        {
            _leopardProfileService = leopardProfileService;
        }
        [HttpGet]
        [Authorize(Roles ="4,5,6,7")]
        public async Task<ActionResult<List<LeopardProfile>>> GetAll()
        {
            return await _leopardProfileService.GetAll();
        }
        [HttpGet("{id}")]
        [Authorize(Roles ="4,5,6,7")]
        public async Task<ActionResult<LeopardProfile>> GetById(int id)
        {
            var leopard = await _leopardProfileService.GetById(id);
            if(leopard == null)
            {
                return NotFound(ErrorModel.NotFound());
            }
            return Ok(leopard);
        }
        [HttpGet("search")]
        [Authorize(Roles = "4,5,6,7")]
        [EnableQuery]
        public ActionResult<IQueryable<LeopardProfile>> Search()
        {
            var query = _leopardProfileService.GetQueyable();
            return Ok(query);
        }
        [HttpPost]
        [Authorize(Roles ="5,6")]
        public async Task<ActionResult<LeopardProfile>> CreateLeopard(CreateModel createModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorModel.Invalid());
                }
                var result = await _leopardProfileService.Create(createModel);
                return StatusCode(201, result);
            }catch(Exception ex)
            {
                return BadRequest(ErrorModel.ServerError());
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> UpdateLeopard(UpdateModel updateModel, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorModel.Invalid());
                }
                var result = await _leopardProfileService.Update(updateModel, id);
                return StatusCode(201, result);
            }catch(Exception ex)
            {
                return BadRequest(ErrorModel.ServerError());
            }
        }
        [HttpDelete]
        [Authorize(Roles = "5,6")]
        public async Task<IActionResult> DeleteLeopard(int id)
        {
            var result = await _leopardProfileService.Delete(id);
            return Ok(result);
        }
    }
}
