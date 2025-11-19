
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Reflection.PortableExecutable;
using UOW;

namespace Services
{
    public class LeopardProfileService 
    {
        private readonly UnitOfWork _unitOfWork;

        public LeopardProfileService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ListLeopardProfileModel> GetAlLeopardProfile()
        {
            var list = _unitOfWork.GetRepository<LeopardProfile>()
                .Entities
                .Select(x => new ListLeopardProfileModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardTypeId = x.LeopardTypeId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                    Origin = x.LeopardType.Origin,
                    Description = x.LeopardType.Description
                })
                .ToList();

            return list;
        }

        public ListLeopardProfileModel GetLeopardProfileId(int id)
        {
            var item = _unitOfWork.GetRepository<LeopardProfile>()
                .Entities
                .Select(x => new ListLeopardProfileModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardTypeId = x.LeopardTypeId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                    Origin = x.LeopardType.Origin,
                    Description = x.LeopardType.Description
                })
                .FirstOrDefault(x => x.LeopardProfileId == id);

            return item; 
        }

        public LeopardProfile CreateLeopardProfile(CreateLeopardProfileModel model)
        {
            try
            {

           
            int maxId = _unitOfWork.GetRepository<LeopardProfile>().Entities.Max(h => (int?)h.LeopardProfileId) ?? 0;
            var item = new LeopardProfile
            {
                LeopardProfileId = model.LeopardProfileId == null ? maxId+1 : model.LeopardProfileId,
                LeopardTypeId = model.LeopardTypeId,
                LeopardName = model.LeopardName,
                Weight = model.Weight,
                Characteristics = model.Characteristics,
                CareNeeds = model.CareNeeds,
                ModifiedDate = model.ModifiedDate == null ? DateTime.Now : model.ModifiedDate
            };
            _unitOfWork.GetRepository<LeopardProfile>().Add(item);
            _unitOfWork.Save();
            return item;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool UpdateLeopardProfile(int id, CreateLeopardProfileModel model)
        {
            var item = _unitOfWork.GetRepository<LeopardProfile>().GetById(id);
            if (item == null)
                return false;
            item.LeopardProfileId = id;
            item.LeopardTypeId = model.LeopardTypeId;
            item.LeopardName = model.LeopardName;
            item.Weight = model.Weight;
            item.Characteristics = model.Characteristics;
            item.CareNeeds = model.CareNeeds;
            item.ModifiedDate = model.ModifiedDate == null ? DateTime.Now : model.ModifiedDate;
            _unitOfWork.GetRepository<LeopardProfile>().Update(item);
            _unitOfWork.Save();

            return true;
        }

        public bool DeleteLeopardProfile(int id)
        {
            var item = _unitOfWork.GetRepository<LeopardProfile>().GetById(id);
            if(item == null)
                return false; 

            _unitOfWork.GetRepository<LeopardProfile>().Delete(item);
            _unitOfWork.Save();

            return true;
        }

        public IQueryable<ListLeopardProfileModel> SearchWithProjection(string? leopardName, double? weight)
        {
            var query = _unitOfWork.GetRepository<LeopardProfile>().Entities
                .Include(h => h.LeopardType)
                .Select(x => new ListLeopardProfileModel
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardTypeId = x.LeopardTypeId,
                    LeopardName = x.LeopardName,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate,
                    LeopardTypeName = x.LeopardType.LeopardTypeName,
                    Origin = x.LeopardType.Origin,
                    Description = x.LeopardType.Description
                });

            if (!string.IsNullOrEmpty(leopardName))
            {
                query = query.Where(h => h.LeopardName.Contains(leopardName));
            }

            if (weight != null)
            {
                query = query.Where(h => h.Weight == weight);
            }
            return query;
        }

    }
}
