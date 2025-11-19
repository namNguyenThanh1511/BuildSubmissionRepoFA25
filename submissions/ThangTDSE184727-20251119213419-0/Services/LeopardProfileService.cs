using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Models;
using Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardProfileService
    {
        private readonly LeopardProfileRepository _leopardProfileRepository;

        public LeopardProfileService()
        {
            _leopardProfileRepository = new LeopardProfileRepository();
        }
        public async Task<List<LeopardProfile>> GetAll()
        {
            var leopardList = await _leopardProfileRepository.GetAllAsync();
            return leopardList;
        }
        public IQueryable<LeopardProfile> GetQueyable()
        {
            return _leopardProfileRepository.GetQueryable().Include(l => l.LeopardType);
        }
        public async Task<LeopardProfile> GetById(int id)
        {
            return await _leopardProfileRepository.GetByIdWithIncludeAsync(id, "LeopardProfileId", l => l.LeopardType);
        }
        public async Task<int> Create(CreateModel createModel)
        {
            var maxId = await _leopardProfileRepository.GetMaxLeopard();
            var newID = maxId + 1;
            var leopard = new LeopardProfile()
            {
                //LeopardProfileId = newID,
                LeopardTypeId = createModel.LeopardTypeId,
                LeopardName = createModel.LeopardName,
                Weight = createModel.Weight,
                Characteristics = createModel.Characteristics,
                CareNeeds = createModel.CareNeeds,
                ModifiedDate = createModel.ModifiedDate,
            };
            return await _leopardProfileRepository.CreateAsync(leopard);
        }
        public async Task<LeopardProfile> Update(UpdateModel updateModel, int id)
        {
            var existingLeopard = await _leopardProfileRepository.GetByIdAsync(id);
            if(existingLeopard == null)
            {
                return null;
            }
            var updateLeopard = new LeopardProfile()
            {
                LeopardTypeId = updateModel.LeopardTypeId,
                LeopardName = updateModel.LeopardName,
                Weight = updateModel.Weight,
                Characteristics = updateModel.Characteristics,
                CareNeeds = updateModel.CareNeeds,
                ModifiedDate = updateModel.ModifiedDate,
            };
            await _leopardProfileRepository.UpdateAsync(updateLeopard);
            var result = await _leopardProfileRepository.GetByIdAsync(id);
            return result;
        }
        public async Task<bool> Delete(int id)
        {
            var leopard = await _leopardProfileRepository.GetByIdAsync(id);
            return await _leopardProfileRepository.RemoveAsync(leopard);
        }
    }
}
