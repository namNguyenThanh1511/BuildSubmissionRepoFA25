using PRN231_SU25_SE184930.bll.Interfaces;
using PRN231_SU25_SE184930.dal.DTOs;
using PRN231_SU25_SE184930.dal.Interfaces;
using PRN231_SU25_SE184930.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184930.bll.Services
{
    public class LeopardService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _leopardProfileRepository;
        private readonly ILeopardTypeRepository _leopardTypeRepository;

        public LeopardService(ILeopardProfileRepository leopardProfileRepository, ILeopardTypeRepository leopardTypeRepository)
        {
            _leopardProfileRepository = leopardProfileRepository;
            _leopardTypeRepository = leopardTypeRepository;
        }

        public async Task<IEnumerable<LeopardProfileReponseDto>> GetAllAsync()
        {
            var leopards = await _leopardProfileRepository.GetAllAsync();
            return leopards.Select(MapToResponseDto);
        }

        public async Task<LeopardProfileReponseDto> GetByIdAsync(int id)
        {
            var leopard = await _leopardProfileRepository.GetByIdAsync(id);
            return leopard == null ? null : MapToResponseDto(leopard);
        }

        public async Task<LeopardProfileReponseDto> CreateAsync(LeopardProfileRequestDto request)
        {
            if (!await _leopardTypeRepository.ExistsAsync(request.LeopardTypeId))
            {
                throw new ArgumentException("LeopartType not found");
            }

            var leopard = new LeopardProfile
            {
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics  = request.Characteristics,
                CareNeeds = request.CareNeeds,
                LeopardTypeId = request.LeopardTypeId
            };

            var createdLeopard = await _leopardProfileRepository.CreateAsync(leopard);
            return MapToResponseDto(createdLeopard);
        }

        private LeopardProfileReponseDto MapToResponseDto(LeopardProfile leopard)
        {
            return new LeopardProfileReponseDto
            {
                LeopardProfileId = leopard.LeopardProfileId,
                LeopardName = leopard.LeopardName,
                Weight = leopard.Weight,
                Characteristics = leopard.Characteristics,
                CareNeeds = leopard.CareNeeds,
                ModifiedDate = leopard.ModifiedDate,
                LeopardTypeId = leopard.LeopardTypeId,
                LeopardType = leopard.LeopardType == null ? null : new LeopardTypeResponseDto
                {
                    LeopardTypeId = leopard.LeopardType.LeopardTypeId,
                    LeopardTypeName = leopard.LeopardType.LeopardTypeName,
                    Description = leopard.LeopardType.Description,
                    Origin = leopard.LeopardType.Origin,
                }
            };
        }
    }
}
