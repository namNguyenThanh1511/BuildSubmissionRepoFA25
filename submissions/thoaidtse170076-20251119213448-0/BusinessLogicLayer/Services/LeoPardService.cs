using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using DataAccessLayer.UOW;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class LeoPardService
    {
        private readonly UnitOfWork _unitOfWork;

        public LeoPardService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateLeopard(CreateLeoPardModel model)
        {
            if (!_unitOfWork.GetRepository<LeopardType>().Entities.Any(b => b.LeopardTypeId == model.LeopardTypeId))
                throw new ArgumentException("Invalid or unauthorized LeopardTypeId", nameof(model.LeopardTypeId));

            if (model.Weight < 15)
                throw new ArgumentException("Weight at least 15");

            var maxId = _unitOfWork.GetRepository<LeopardProfile>().Entities.Max(h => (int?)h.LeopardProfileId) ?? 0;
            var leopard = new LeopardProfile
            {
                LeopardProfileId = maxId + 1,
                LeopardName = model.LeopardName,
                Weight = model.Weight,
                Characteristics = model.Characteristics,
                CareNeeds = model.CareNeeds,
                ModifiedDate = (DateTime.Now)
            };
            _unitOfWork.GetRepository<LeopardProfile>().Add(leopard);
            _unitOfWork.Save();
        }

        public void DeleteLeoPard(int id)
        {
            var leopard = _unitOfWork.GetRepository<LeopardProfile>().Entities.FirstOrDefault(h => h.LeopardProfileId == id);
            if (leopard == null)
                throw new ArgumentException("Shoe not found");

            _unitOfWork.GetRepository<LeopardProfile>().Delete(leopard);
            _unitOfWork.Save();
        }

        public List<LeoPardModel> GetAllLeoPard()
        {
            var list = _unitOfWork.GetRepository<LeopardProfile>()
              .Entities
              .Select(x => new LeoPardModel
              {
                  LeopardProfileId = x.LeopardProfileId,
                  LeopardName = x.LeopardName,
                  Weight = x.Weight,
                  Characteristics = x.Characteristics,
                  CareNeeds = x.CareNeeds,
                  ModifiedDate = x.ModifiedDate,
                  LeopardTypeId = x.LeopardTypeId,
                  LeopardTypeName = x.LeopardType.LeopardTypeName,
                  Origin = x.LeopardType.Origin,
                  Description = x.LeopardType.Description,
              })
              .ToList();

            return list;
        }

        public LeoPardModel GetLeoPardId(int id)
        {
            var leopard = _unitOfWork.GetRepository<LeopardProfile>()
                .Entities
                .Select(x => new LeoPardModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeId = x.LeopardTypeId,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                    Origin = x.LeopardType.Origin,
                    Description = x.LeopardType.Description,
                })
                .FirstOrDefault(x => x.LeopardProfileId == id);
            return leopard;
        }

        public IQueryable SearchWithProjection(string? leopardname, string? weight)
        {
            var query = _unitOfWork.GetRepository<LeopardProfile>().Entities
               .Include(h => h.LeopardType)
               .Select(h => new LeoPardModel
               {
                   LeopardProfileId = h.LeopardProfileId,
                   LeopardName = h.LeopardName,
                   Weight = h.Weight,
                   Characteristics = h.Characteristics,
                   CareNeeds = h.CareNeeds,
                   ModifiedDate = h.ModifiedDate,
                   LeopardTypeId = h.LeopardTypeId,
                   LeopardTypeName = h.LeopardType.LeopardTypeName,
                   Origin = h.LeopardType.Origin,
                   Description = h.LeopardType.Description,
               });

            // Lọc bằng query thường
            if (!string.IsNullOrEmpty(leopardname))
            {
                query = query.Where(h => h.LeopardName.Contains(leopardname));
            }

            return query;
        }

        public void UpdateLeoPard(int id, CreateLeoPardModel model)
        {
            var leopard = _unitOfWork.GetRepository<LeopardProfile>().Entities.FirstOrDefault(h => h.LeopardProfileId == id);

            if (leopard == null)
                throw new ArgumentException("Not found");

            _unitOfWork.GetRepository<LeopardProfile>().Update(leopard);
            _unitOfWork.Save();
        }
    }
}
