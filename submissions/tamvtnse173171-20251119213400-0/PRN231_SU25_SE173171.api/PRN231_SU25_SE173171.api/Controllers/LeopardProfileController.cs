using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE173171.BLL.Base;
using PRN231_SU25_SE173171.BLL.DTOs;
using PRN231_SU25_SE173171.BLL.Interfaces;
using PRN231_SU25_SE173171.BLL.Store;

namespace PRN231_SU25_SE173171.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _service;

        public LeopardProfileController(ILeopardProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "5,6,7,4")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _service.GetAllList());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "5,6,7,4")]
        public async Task<ActionResult> GetById(int id)
        {
            var item = await _service.GetById(id);
            if (item == null)
            {
                return NotFound(new
                {
                    errorCode = ErrorCode.ErrorCodeResourceNotFound,
                    message = $"Profile with id {id} not found"
                });
            }
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "5,6")]
        public async Task<ActionResult> CreateHandBag(CreateLeopardProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToArray();

                    return BadRequest(new
                    {
                        ErrorCode = ErrorCode.ErrorCodeMissingInvalidInput,
                        Message = string.Join("; ", errorMessages)
                    });
                }

                await _service.Create(request);
                return Ok("Create Successful!");

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseErrorResponse(ErrorCode.ErrorCodeMissingInvalidInput, ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<ActionResult> UpdateProfile(int id, UpdateLeopardProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToArray();

                    return BadRequest(new
                    {
                        ErrorCode = ErrorCode.ErrorCodeMissingInvalidInput,
                        Message = string.Join("; ", errorMessages)
                    });
                }

                await _service.Update(id, request);
                return Ok("Update Successful!");

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseErrorResponse(ErrorCode.ErrorCodeMissingInvalidInput, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "5,6")]
        public async Task<ActionResult> DeleteProfile(int id)
        {
            try
            {
                await _service.Delete(id);
                return Ok("Delete Successful!");
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseErrorResponse(ErrorCode.ErrorCodeMissingInvalidInput, ex.Message));
            }
        }

        [HttpGet("search")]
        [Authorize]
        [EnableQuery]
        public async Task<ActionResult> Search(string? LeopardName, double Weight)
        {
            return Ok();
        }
    }
}
