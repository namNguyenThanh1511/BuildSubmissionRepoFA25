using BusinessObjects;
using BusinessObjects.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Services;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace PRN231_SU25_SE182328.api.Controllers
{
    [Route("api/LeopardProfile")]
    public class LeopardProfileController : BaseController
    {
        private readonly ILeopardProfileService _LeopardProfileService;
        private readonly ILeopardTypeService _LeopardTypeService;

        public LeopardProfileController(ILeopardProfileService LeopardProfileService, ILeopardTypeService LeopardTypeService)
        {
            _LeopardProfileService = LeopardProfileService;
            _LeopardTypeService = LeopardTypeService;
        }

        [HttpGet]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult GetAll()
        {
            try
            {
                var LeopardProfiles = _LeopardProfileService.GetAllLeopardProfiles()
                    .Select(h => new LeopardProfileDto
                    {
                        LeopardProfileId = h.LeopardProfileId,
                        LeopardTypeId = h.LeopardTypeId,
                        LeopardName = h.LeopardName,
                        Weight = h.Weight,
                        Characteristics = h.Characteristics,
                        CareNeeds = h.CareNeeds,
                        ModifiedDate = h.ModifiedDate,
                    })
                    .ToList();

                return Ok(LeopardProfiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetAll] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }

        // GET /api/LeopardProfiles/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult GetById(int id)
        {
            try
            {
                var LeopardProfile = _LeopardProfileService.GetLeopardProfileById(id);
                if (LeopardProfile == null)
                    return Error("HB40401", "Leopard Profile not found", 404);

                // Chuyển sang trả về DTO
                var dto = new LeopardProfileDto
                {
                    LeopardProfileId = LeopardProfile.LeopardProfileId,
                    LeopardTypeId = LeopardProfile.LeopardTypeId,
                    LeopardName = LeopardProfile.LeopardName,
                    Weight = LeopardProfile.Weight,
                    Characteristics = LeopardProfile.Characteristics,
                    CareNeeds = LeopardProfile.CareNeeds,
                    ModifiedDate = LeopardProfile.ModifiedDate,
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][GetById] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }

        public class LeopardProfileCreateRequest
        {
            public int LeopardProfileId { get; set; }
            public int LeopardTypeId { get; set; }
            public string LeopardName { get; set; }
            public double Weight { get; set; }
            public string Characteristics { get; set; }
            public string CareNeeds { get; set; }
            public DateTime ModifiedDate { get; set; }
        }

        public class LeopardProfileUpdateRequest
        {
            public int LeopardTypeId { get; set; }
            public string LeopardName { get; set; }
            public double Weight { get; set; }
            public string Characteristics { get; set; }
            public string CareNeeds { get; set; }
            public DateTime ModifiedDate { get; set; }
        }

        // POST /api/LeopardProfiles
        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Create([FromBody] LeopardProfileCreateRequest request)
        {
            try
            {
                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (string.IsNullOrWhiteSpace(request.LeopardName))
                    return Error("HB40001", "leopardName is required", 400);
                if (!regex.IsMatch(request.LeopardName))
                    return Error("HB40001", "leopardName is invalid", 400);
                if (string.IsNullOrWhiteSpace(request.Characteristics))
                    return Error("HB40001", "characteristics is required", 400);
                if (string.IsNullOrWhiteSpace(request.CareNeeds))
                    return Error("HB40001", "careNeeds is required", 400);
                if (request.ModifiedDate == default)
                    return Error("HB40001", "modifiedDate is required", 400);
                if (request.ModifiedDate > DateTime.Now)
                    return Error("HB40001", "modifiedDate cannot be in the future", 400);
                if (request.Weight <= 0)
                    return Error("HB40001", "weight must be greater than 0", 400);
                if (request.LeopardTypeId <= 0)
                    return Error("HB40001", "leopardTypeId is required and must be greater than 0", 400);

                // Kiểm tra LeopardTypeId có tồn tại không
                var LeopardType = _LeopardTypeService.GetLeopardTypeById(request.LeopardTypeId);
                if (LeopardType == null)
                    return Error("HB40001", "leopardTypeId does not exist", 400);

                if (_LeopardProfileService.GetLeopardProfileByName(request.LeopardName) != null)
                    return Error("HB40001", "leopardName already exists", 400);

                var LeopardProfile = new LeopardProfile
                {
                    LeopardProfileId = request.LeopardProfileId,
                    LeopardTypeId = request.LeopardTypeId,
                    LeopardName = request.LeopardName,
                    Weight = request.Weight,
                    Characteristics = request.Characteristics,
                    CareNeeds = request.CareNeeds,
                    ModifiedDate = request.ModifiedDate,
                };

                _LeopardProfileService.AddLeopardProfile(LeopardProfile);

                var created = _LeopardProfileService.GetLeopardProfileByName(request.LeopardName);

                return StatusCode(201, new
                {
                    id = created?.LeopardProfileId,
                    message = "Leopard Profile created successfully"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][Create] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }

        // PUT /api/LeopardProfiles/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Update(int id, [FromBody] LeopardProfileUpdateRequest request)
        {
            try
            {
                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (string.IsNullOrWhiteSpace(request.LeopardName))
                    return Error("HB40001", "leopardName is required", 400);
                if (!regex.IsMatch(request.LeopardName))
                    return Error("HB40001", "leopardName is invalid", 400);
                if (string.IsNullOrWhiteSpace(request.Characteristics))
                    return Error("HB40001", "characteristics is required", 400);
                if (string.IsNullOrWhiteSpace(request.CareNeeds))
                    return Error("HB40001", "careNeeds is required", 400);
                if (request.ModifiedDate == default)
                    return Error("HB40001", "modifiedDate is required", 400);
                if (request.ModifiedDate > DateTime.Now)
                    return Error("HB40001", "modifiedDate cannot be in the future", 400);
                if (request.Weight <= 0)
                    return Error("HB40001", "weight must be greater than 0", 400);
                if (request.LeopardTypeId <= 0)
                    return Error("HB40001", "leopardTypeId is required and must be greater than 0", 400);

                var LeopardType = _LeopardTypeService.GetLeopardTypeById(request.LeopardTypeId);
                if (LeopardType == null)
                    return Error("HB40001", "LeopardTypeId does not exist", 400);

                var existing = _LeopardProfileService.GetLeopardProfileById(id);
                if (existing == null)
                    return Error("HB40401", "Leopard Profile not found", 404);

                existing.LeopardName = request.LeopardName;
                existing.Weight = request.Weight;
                existing.Characteristics = request.Characteristics;
                existing.CareNeeds = request.CareNeeds;
                existing.LeopardTypeId = request.LeopardTypeId;
                existing.ModifiedDate = request.ModifiedDate;

                _LeopardProfileService.UpdateLeopardProfile(existing);
                return Ok(new
                {
                    id = existing.LeopardProfileId,
                    message = "Leopard Profile updated successfully"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][Update] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }

        // DELETE /api/LeopardProfiles/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existing = _LeopardProfileService.GetLeopardProfileById(id);
                if (existing == null)
                    return Error("HB40401", "Leopard Profile not found", 404);

                _LeopardProfileService.DeleteLeopardProfile(id);
                return Ok(new
                {
                    message = "Leopard Profile deleted successfully"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][Delete] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }

        // GET /api/LeopardProfiles/search?leopardName=...&weight=...
        [HttpGet("search")]
        [EnableQuery]
        [Authorize]
        public IActionResult Search([FromQuery] string? leopardName, [FromQuery] int Weight)
        {
            try
            {
                var LeopardProfiles = _LeopardProfileService.GetAllLeopardProfiles().AsQueryable();

                if (!string.IsNullOrWhiteSpace(leopardName))
                    LeopardProfiles = LeopardProfiles.Where(h => h.LeopardName != null && h.LeopardName.Contains(leopardName, StringComparison.OrdinalIgnoreCase));
                if (Weight > 0)
                    LeopardProfiles = LeopardProfiles.Where(h => h.Weight != null && h.Weight == Weight);

                var grouped = LeopardProfiles
                    .Where(h => h.LeopardType != null)
                    .GroupBy(h => h.LeopardType!.LeopardTypeName)
                    .Select(g => new
                    {
                        LeopardTypeName = g.Key,
                        LeopardProfiles = g.Select(h => new LeopardProfileDto
                        {
                            LeopardProfileId = h.LeopardProfileId,
                            LeopardTypeId = h.LeopardTypeId,
                            LeopardName = h.LeopardName,
                            Weight = h.Weight,
                            Characteristics = h.Characteristics,
                            CareNeeds = h.CareNeeds,
                            ModifiedDate = h.ModifiedDate,
                        }).ToList()
                    })
                    .ToList();

                return Ok(grouped);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR][Search] {ex}");
                return Error("HB50001", "Internal server error", 500);
            }
        }
    }
}
