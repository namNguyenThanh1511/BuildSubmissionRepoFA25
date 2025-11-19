using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service;

namespace PRN231_SU25_SE161141.Controllers
{
    [Route("/api/LeoPardProfile")]
    [ApiController]
    public class LeopardProfileController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly LeopardProfileService _leopardProfileService;

        public LeopardProfileController(LoginService loginService, LeopardProfileService leopardProfileService)
        {
            _loginService = loginService;
            _leopardProfileService = leopardProfileService;
        }

        [HttpPost("/api/auth")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _loginService.Login(email, password);
            if (user == null) return BadRequest("Wrong email or pasword");
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> ListAllLeopardProfile()
        {
            var lp = await _leopardProfileService.ListAllLeopardProfile();
            return Ok(lp);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetLeopardProfile(int id)
        {
            var lp = await _leopardProfileService.GetLeopardProfile(id);
            if (lp == null) return NotFound("Not found");
            return Ok(lp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeopardProfile(LeopardProfile leopardProfile)
        {
            var result = await _leopardProfileService.CreateProfile(leopardProfile);
            if (!result) return BadRequest("Failed");
            return Ok("Success");
        }

        [HttpPut("/{LeopardProfileId}")]
        public async Task<IActionResult> updateLeopardProfile(LeopardProfile leopardProfile)
        {
            var result = await _leopardProfileService.UpdateProfile(leopardProfile);
            if (result == 400) return BadRequest("Failed");
            if (result == 404) return NotFound("Not found");
            return Ok("Success");
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> RemoveLeopardProfile(int id)
        {
            var result = await _leopardProfileService.DeleteProfile(id);
            if (!result) return BadRequest("Failed");
            return Ok("Success");
        }
    }
}
