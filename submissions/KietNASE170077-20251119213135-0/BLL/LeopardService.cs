using DAL.Entities;
using DAL.Model;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LeopardService
    {
        private readonly UnitOfWork _unitOfWork;

        public LeopardService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ListAllModel> GetAll()
        {
            var list = _unitOfWork.GetRepository<LeopardProfile>()
                .Entities
                .Select(x => new ListAllModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                })
                .ToList();

            return list;
        }

        public ListAllModel GetById(int id)
        {
            var entity = _unitOfWork.GetRepository<LeopardProfile>()
                .Entities
                .Select(x => new ListAllModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                })
                .FirstOrDefault(x => x.LeopardProfileId == id);
            return entity;
        }

        public void Create(CreateModel x)
        {
            try
            {
                var entity = new LeopardProfile
                {
                    //LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeId = x.LeopardTypeId,
                };
                _unitOfWork.GetRepository<LeopardProfile>().Add(entity);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }

        public bool Update(int id, UpdateModel model)
        {
            var entity = _unitOfWork.GetRepository<LeopardProfile>().GetById(id);
            if (entity == null)
            {
                return false;
            }

            entity.Weight = model.Weight;
            entity.Characteristics = model.Characteristics;
            entity.CareNeeds = model.CareNeeds;
            entity.ModifiedDate = model.ModifiedDate;
            entity.LeopardTypeId = model.LeopardTypeId;
            entity.LeopardName = model.LeopardName;

            _unitOfWork.GetRepository<LeopardProfile>().Update(entity);
            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var entity = _unitOfWork.GetRepository<LeopardProfile>().GetById(id);
            if (entity == null)
            {
                return false;
            }

            _unitOfWork.GetRepository<LeopardProfile>().Delete(entity);
            _unitOfWork.Save();

            return true;
        }

        public IQueryable<ListAllModel> SearchWithProjection(string? name, double? weight)
        {
            var query = _unitOfWork.GetRepository<LeopardProfile>().Entities
                .Include(h => h.LeopardType)
                .Select(x => new ListAllModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                });

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(h => h.LeopardName.Contains(name));
            }

            if (weight != null)
            {
                query = query.Where(h => h.Weight == weight);
            }

            return query;
        }
    }
}
