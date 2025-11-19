using Repositories.Interfaces;
using Repositories.Models;
using Services.Models.Requests.Profiles;
using Services.Models.Responses.LeopardProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LeopardProfileServices
    {
        IUnitOfWork _unitOfWork;
        public LeopardProfileServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ProfileResponse>> GetAll()
        {
            var profiles = await _unitOfWork.Profiles.GetAllWithIncludesAsync(h => h.LeopardType);
            return profiles.Select(h => new ProfileResponse
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                Weight = h.Weight,
                LeopardName= h.LeopardName,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate,
                leopardType = h.LeopardType?.LeopardTypeName ?? string.Empty,
            });
        }
        public async Task<ProfileResponse?> GetById(int id)
        {
            var h = await _unitOfWork.Profiles.GetByIdWithIncludesAsync(id, h => h.LeopardType);
            if(h == null) 
                return null;
            return new ProfileResponse
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                Weight = h.Weight,
                LeopardName = h.LeopardName,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds,
                ModifiedDate = h.ModifiedDate,
                leopardType = h.LeopardType?.LeopardTypeName ?? string.Empty,
            };
        }
        //public async Task<LeopardProfile?> Create(CreateLeopardProfile profile)
        //{
        //    var type = await _unitOfWork.Types.GetByIdAsync(profile.LeopardTypeId);
        //    if(type == null)
        //    {
        //        throw new ArgumentException("Not found");
        //    }
        //    var existId = await _unitOfWork.Profiles.GetByIdAsync(profile.LeopardProfileId);
        //    if(!existId == null)
        //    {
        //        throw new ArgumentException("profile id existed");
        //    }
        //    var profile = new LeopardProfile
        //    {

        //    }
        //}
    }
}
