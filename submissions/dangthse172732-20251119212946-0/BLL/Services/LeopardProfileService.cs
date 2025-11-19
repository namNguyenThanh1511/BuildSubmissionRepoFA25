using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeopardProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(LeopardProfileCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.LeopardName))
                throw new ArgumentException("Model name is required");

            //model.Validate();

            int id = await Count();
            id++;

        var Leopart = new LeopardProfile
            {
            LeopardName = model.LeopardName,
            Weight = model.Weight,
            Characteristics = model.Characteristics,
            CareNeeds = model.CareNeeds,
            LeopardTypeId = model.LeopardTypeId,
            LeopardProfileId = id,
            ModifiedDate = DateTime.UtcNow,
            };

            await _unitOfWork.GetRepository<LeopardProfile>().InsertAsync(Leopart);
            await _unitOfWork.GetRepository<LeopardProfile>().SaveAsync();
        }

        public async Task Delete(int id)
        {
            var handbag = await _unitOfWork.GetRepository<LeopardProfile>().GetByIdAsync(id);
            if (handbag == null)
                throw new KeyNotFoundException("Handbag not found");

            await _unitOfWork.GetRepository<LeopardProfile>().DeleteAsync(id);
                        await _unitOfWork.GetRepository<LeopardProfile>().SaveAsync();
        }

        public async Task<List<LeopartResponseModel>> Get()
        {
            var model = await _unitOfWork
                .GetRepository<LeopardProfile>()
                .Entities
                .Include(h => h.LeopardType)
                .ToListAsync();

            return model.Select(h => new LeopartResponseModel
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                LeopardTypeId = h.LeopardTypeId,
            }).ToList();
        }

        public async Task<LeopartResponseModel> Get(int id)
        {
            var model = await _unitOfWork.GetRepository<LeopardProfile>().Entities.Include(x => x.LeopardTypeId).FirstOrDefaultAsync(x => x.LeopardProfileId == id);
            if (model == null)
                throw new KeyNotFoundException("Handbag not found");

            return new LeopartResponseModel
            {
                LeopardProfileId = model.LeopardProfileId,
                LeopardName = model.LeopardName,
                Weight = model.Weight,
                Characteristics = model.Characteristics,
                CareNeeds = model.CareNeeds,
                LeopardTypeId = model.LeopardTypeId,
            };
        }

        public async Task Update(int id, LeopardProfileCreateModel model)
        {
            var handbag = await _unitOfWork.GetRepository<LeopardProfile>().GetByIdAsync(id);
            //model.Validate();
            if (handbag == null)
                throw new KeyNotFoundException("Handbag not found");

            handbag.LeopardName = model.LeopardName;
            handbag.Weight = model.Weight;
            handbag.Characteristics = model.Characteristics;
            handbag.CareNeeds = model.CareNeeds;
            handbag.LeopardTypeId = model.LeopardTypeId;
            handbag.LeopardProfileId = id;
            await _unitOfWork.GetRepository<LeopardProfile>().UpdateAsync(handbag);
            await _unitOfWork.GetRepository<LeopardProfile>().SaveAsync();
        }

        public async Task<int> Count()
        {
            return await _unitOfWork.GetRepository<LeopardProfile>().Entities.CountAsync();
        }
    }
}
