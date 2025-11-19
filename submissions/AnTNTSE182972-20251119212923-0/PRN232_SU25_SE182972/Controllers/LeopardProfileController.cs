using BLL.Interface;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232_SU25_SE182972.ErrorModel;

namespace PRN232_SU25_SE182972.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly ILeopardProfileService _leopardProfileService;
        public LeopardProfileController(ILeopardProfileService leopardProfileService)
        {
            _leopardProfileService = leopardProfileService;

        }
        [Authorize(Roles = "4,5,6,7")]
        [HttpGet]
        public async Task<IActionResult> GetAllProfile()
        {
            var list = await _leopardProfileService.GetAllProfile();
            return new OkObjectResult(list);
        }
        [Authorize(Roles = "4,5,6,7")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var profile = await _leopardProfileService.GetProfileById(id);
            return new OkObjectResult(profile);
        }
        [Authorize(Roles = "5,6")]

        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] LeopardProfileModifyDTO leopardProfileModifyDTO)
        {
            var check = _leopardProfileService.ValidateProduct(leopardProfileModifyDTO);
            if (check == false) return ErrorResponse.FromCode("HB40001");
            await _leopardProfileService.CreateNew(leopardProfileModifyDTO);
            return new OkObjectResult("Create Successesfully");
        }
        [Authorize(Roles = "5,6")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] LeopardProfileModifyDTO leopardProfileModifyDTO, int id)
        {
            var profile = await _leopardProfileService.GetProfileById(id);
            if (profile == null) return ErrorResponse.FromCode("HB40401");
            var check = _leopardProfileService.ValidateProduct(leopardProfileModifyDTO);
            if (check == false) return ErrorResponse.FromCode("HB40001");
            await _leopardProfileService.UpdateProfile(leopardProfileModifyDTO, id);
            return new OkObjectResult("Update Successfully");
        }
        [Authorize(Roles = "5,6")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _leopardProfileService.GetProfileById(id);
            if (profile == null) return ErrorResponse.FromCode("HB40401");
            await _leopardProfileService.DeleteProfile(profile.LeopardProfileId);
            return new OkObjectResult("Delete Successfully");
        }
    }
}
