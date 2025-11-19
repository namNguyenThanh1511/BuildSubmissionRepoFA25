using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE172431.BLL.DTO.Response;
using PRN231_SU25_SE172431.DAL.Data;
using PRN231_SU25_SE172431.DAL.Entities;

namespace PRN231_SU25_SE172431.BLL.Service
{
    public interface ILeopardProfileService
    {
        public Task<List<LeopartProfileResponse>> GetAll();
        public Task<LeopartProfileResponse?> GetById(int id);
        public void CreateLeopardProfileAsync(LeopardProfile request);
        public void UpdateLeopardProfileAsync(LeopardProfile request, int id);
        public Task DeleteLeopardProfileAsync(int id);
    }
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LeopardProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateLeopardProfileAsync(LeopardProfile request)
        {
           
            _unitOfWork.LeopardProfileRepository.Insert(request);
            _unitOfWork.Save();
        }

        public async Task DeleteLeopardProfileAsync(int id)
        {
           var existed = await _unitOfWork.LeopardProfileRepository.Entities.FirstOrDefaultAsync(x => x.LeopardProfileId == id);
            if (existed == null)
            {
                throw new Exception("Leopard profile not found");
            }
            _unitOfWork.LeopardProfileRepository.Delete(existed);
            _unitOfWork.Save();
        }

        public async Task<List<LeopartProfileResponse>> GetAll()
        {
           var result =  await _unitOfWork.LeopardProfileRepository.Entities.Include(x=>x.LeopardType).ToListAsync();
            var response = result.Select(x => new LeopartProfileResponse
            {
                LeopardProfileId = x.LeopardProfileId,
                LeopardTypeId = x.LeopardTypeId,
                LeopardTypeName = x.LeopardType.LeopardTypeName,
                LeopardName = x.LeopardName,
                Weight = x.Weight,
                Characteristics = x.Characteristics,
                CareNeeds = x.CareNeeds,
                ModifiedDate = x.ModifiedDate
            }).ToList();
            return response;
        }

        public async Task<LeopartProfileResponse?> GetById(int id)
        {
            var h = await _unitOfWork.LeopardProfileRepository.Entities.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == id);

            if (h == null)
            {
                return null;
            }
            var response = new LeopartProfileResponse
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                LeopardTypeName = h.LeopardType.LeopardTypeName,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate
            };
            return response;

        }

        public void UpdateLeopardProfileAsync(LeopardProfile request, int id)
        {
            throw new NotImplementedException();
        }
    }
}
