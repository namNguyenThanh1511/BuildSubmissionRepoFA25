using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_SU25_SE183867.service;
using PRN232_SU25_SE183867.service.Dto;

namespace PRN232_SU25_SE183867.api.Controllers
{
    [ApiController]
    [Route("api/LeopardProfile")]
    public class ProfileController : BaseController
    {
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [Authorize(Roles = "4,5,6,7")]
        [HttpGet]
        public async Task<IActionResult>  GetAll ()
        {
            try
            {
                return Ok(await _profileService.GetAllAsync());

            } catch( Exception e)
            {
                return HandleException(e);
            }
        }


        [Authorize(Roles = "4,5,6,7")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _profileService.GetById(id));

            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }


        [Authorize(Roles = "5,6")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileReq request)
        {
            try
            {
                await _profileService.CreateProfile(request);
                return StatusCode(201, new { message = "Profile added." });

            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [Authorize(Roles = "5,6")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] int id, [FromBody] CreateProfileReq request)
        {
            try
            {
                await _profileService.UpdateProfile(id, request);
                return StatusCode(200, new { message = "Profile update." });

            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [Authorize(Roles = "5,6")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile([FromRoute] int id)
        {
            try
            {
                await _profileService.DeleteProfile(id);
                return StatusCode(200, new { message = "Profile deleted." });

            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
    }
}
