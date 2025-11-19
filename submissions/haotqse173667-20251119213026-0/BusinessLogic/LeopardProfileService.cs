using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interface;
using BusinessLogic.ModalViews;
using DataAccess.Interface;
using DataAccess.Models;

namespace BusinessLogic
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repository;

        public LeopardProfileService(ILeopardProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Create(LeopardProfileRequest request)
        {
            var handbag = new LeopardProfile

            {
                LeopardProfileId = new Random().Next(1000, 9999), 
                LeopardName = request.LeopardName,
                Characteristics = request.Characteristic,
                Weight = request.Weight,
                CareNeeds = request.CareNeeds,
                LeopardTypeId = request.TypeId,
                ModifiedDate = request.ModifiedDate,
            };
            await _repository.AddAsync(handbag);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var handbag = await _repository.GetByIdAsync(id);
            if (handbag == null) return false;

            await _repository.DeleteAsync(handbag);
            return true;
        }

        public async  Task<List<LeopardProfileReponse>> GetAll()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(h => new LeopardProfileReponse
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardName = h.LeopardName,
                Characteristics = h.Characteristics,
                Weight = h.Weight ,
                CareNeeds = h.CareNeeds,
                TypeId = h.LeopardTypeId,
                ModifiedDate = h.ModifiedDate,
              
            }).ToList();
        }

        public async Task<LeopardProfileReponse> GetById(int id)
        {
            var h = await _repository.GetByIdAsync(id);
            if (h == null) return null;
            return new LeopardProfileReponse
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardName = h.LeopardName,
                Characteristics = h.Characteristics,
                Weight = h.Weight,
                CareNeeds = h.CareNeeds,
                TypeId = h.LeopardTypeId,
                ModifiedDate = h.ModifiedDate,
            };
        }

        public async Task<List<LeopardProfileReponse>> Search(string modelName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(int id, LeopardProfileRequest request)
        {
            var handbag = await _repository.GetByIdAsync(id);
            if (handbag == null) return false;

            handbag.LeopardName = request.LeopardName;
            handbag.Characteristics = request.Characteristic;
            handbag.Weight = request.Weight;
            handbag.ModifiedDate = request.ModifiedDate;
            handbag.CareNeeds = request.CareNeeds;
            handbag.LeopardTypeId = request.TypeId;

            await _repository.UpdateAsync(handbag);
            return true;
        }
    }
}
