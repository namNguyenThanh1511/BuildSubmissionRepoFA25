using SU25Leopard.BusinessObject.DTO;
using SU25Leopard.BusinessObject.Models;
using SU25Leopard.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SU25Leopard.BusinessLogicLayer.Services
{
    public class LeopardProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LeopardProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<LeopardProfile>> GetAllLeopardProfileAsync()
        {
            return await _unitOfWork.LeopardProfileRepository.GetAllAsync();
        }
        public async Task<LeopardProfile?> GetLeopardProfileByIdAsync(int id)
        {
            return await _unitOfWork.LeopardProfileRepository.GetByIdAsync(id);
        }
        public async Task<LeopardProfile> AddLeopardProfileAsync(LeopardProfileRequestDTO dto)
        {
            LeopardProfile leopardProfile = new LeopardProfile
            {
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate
            };

            var leopardTypeExists = await _unitOfWork.LeopardTypeRepository.GetSingleByConditionAsync(predicate: x => x.LeopardTypeId == dto.LeopardTypeId);
            if (leopardTypeExists == null)
            {
                return null;
            }

            var leopardProfileExists = await _unitOfWork.LeopardProfileRepository.GetByIdAsync(leopardProfile.LeopardProfileId);

            if (leopardProfileExists != null)
            {
                return null;
            }
            var result = await _unitOfWork.LeopardProfileRepository.AddAsync(leopardProfile);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
        public async Task<LeopardProfile> UpdateLeopardProfileAsync(LeopardProfileRequestDTO dto, int leopardProfileId)
        {
            var leopardProfileExists = await _unitOfWork.LeopardProfileRepository.GetSingleByConditionAsync(predicate: x => x.LeopardProfileId == leopardProfileId);

            if (leopardProfileExists == null)
            {
                return null;
            }

            var leopardTypeExists = await _unitOfWork.LeopardTypeRepository.GetSingleByConditionAsync(predicate: x => x.LeopardTypeId == dto.LeopardTypeId);
            if (leopardTypeExists == null)
            {
                return null;
            }
            leopardProfileExists.LeopardTypeId = dto.LeopardTypeId;
            leopardProfileExists.LeopardName = dto.LeopardName;
            leopardProfileExists.Weight = dto.Weight;
            leopardProfileExists.Characteristics = dto.Characteristics;
            leopardProfileExists.CareNeeds = dto.CareNeeds;
            leopardProfileExists.ModifiedDate = dto.ModifiedDate;

            var result = await _unitOfWork.LeopardProfileRepository.Update(leopardProfileExists);
            await _unitOfWork.SaveChangesAsync();
            if (!result)
            {
                return null;
            }
            return leopardProfileExists;
        }
        public async Task<LeopardProfile> DeleteLeopardProfileAsync(int leopardProfileId)
        {
            LeopardProfile leopardProfileExist = await _unitOfWork.LeopardProfileRepository.GetSingleByConditionAsync(predicate: x => x.LeopardProfileId == leopardProfileId);
            if (leopardProfileExist == null)
            {
                return null;
            }
            var result = await _unitOfWork.LeopardProfileRepository.Remove(predicate: x => x.LeopardProfileId == leopardProfileExist.LeopardProfileId);
            await _unitOfWork.SaveChangesAsync();
            if (!result)
            {
                return null;
            }
            return leopardProfileExist;
        }
        public async Task<Object> SearchLeopardProfile(string leopardName, double weight)
        {

            var leopardProfiles = await _unitOfWork.LeopardProfileRepository.search(leopardName, weight);
            return leopardProfiles;
        }
    }
}
