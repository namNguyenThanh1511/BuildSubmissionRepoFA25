using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PRN231_SU25_SE181580.BLL.DTOs;
using PRN231_SU25_SE181580.BLL.Interfaces;

namespace PRN231_SU25_SE181580.api.Controllers {


    [ApiController]
    [Route("api/LeopardProfile")]
    public class LeopardProfileController: ControllerBase {
        private readonly ILeoPardProfileService _svc;
        public LeopardProfileController(ILeoPardProfileService svc) => _svc = svc;

        // GET /api/LeopardProfile
        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator,Developer,Member")]
        public async Task<IActionResult> GetAll() {

            return Ok(await _svc.GetAllAsync());


        }


        // GET /api/LeopardProfile/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Moderator,Developer,Member")]
        public async Task<IActionResult> GetById(int id) {
            var dto = await _svc.GetByIdAsync(id);
            if (dto is null)
                return NotFound(new ErrorResponse("HB40401", "Handbag not found"));
            return Ok(dto);
        }

        // POST /api/LeopardProfile
        [HttpPost]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Create([FromBody] LeopardProfileDTO dto) {
            try {
                var created = await _svc.CreateAsync(dto);
                return StatusCode(201, created);
            } catch (ArgumentException ex) {
                return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
            } catch {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal server error"));
            }
        }

        // PUT /api/LeopardProfile/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] LeopardProfileDTO dto) {
            try {
                var updated = await _svc.UpdateAsync(id, dto);
                if (updated is null)
                    return NotFound(new ErrorResponse("HB40401", "Handbag not found"));
                return Ok(updated);
            } catch (ArgumentException ex) {
                return BadRequest(new ErrorResponse("HB40001", "Missing/invalid input"));
            } catch {
                return StatusCode(500, new ErrorResponse("HB50001", "Internal server error"));
            }
        }

        // DELETE /api/LeopardProfile/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int id) {
            var ok = await _svc.DeleteAsync(id);
            if (!ok)
                return NotFound(new ErrorResponse("HB40401", "Handbag not found"));
            return Ok();
        }

        // GET /api/LeopardProfile/search?1=..&2=..
        [HttpGet("search")]
        [EnableQuery]
        [Authorize]
        public IQueryable<LeopardProfileDTO> SearchOData() {
            return _svc.AsQueryable();
        }

        public class ErrorResponse {
            public string ErrorCode { get; }
            public string Message { get; }

            public ErrorResponse(string errorCode, string message) {
                ErrorCode = errorCode;
                Message = message;
            }
        }
    }

}
