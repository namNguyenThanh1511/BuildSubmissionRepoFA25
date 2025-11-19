using Repository;
using Repository.DTO;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LeopardProfileService
    {
        private readonly LeopardProfileRepository _repo;

        public LeopardProfileService(LeopardProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<(int code, LeopardProfileView item)> Create(LeopardProfileCreate obj)
        {
            /*var existing = await _repo.GetById(obj.LeopardProfileId);

            if(existing != null)
            {
                return (400, null);
            }*/

            var leopardProfile = new LeopardProfile
            {
                LeopardProfileId = 0,
                LeopardTypeId = obj.LeopardTypeId,
                LeopardName = obj.LeopardName,
                Weight = obj.Weight,
                Characteristics = obj.Characteristics,
                CareNeeds = obj.CareNeeds,
                ModifiedDate = DateTime.UtcNow
            };

            var result = await _repo.Create(leopardProfile);

            if (result < 1)
                return (500, null);

            var id = _repo.GetLatestId();
            leopardProfile = await _repo.GetById(id);

            return (200, new LeopardProfileView
            {
                LeopardProfileId = leopardProfile.LeopardProfileId,
                LeopardTypeId = leopardProfile.LeopardTypeId,
                LeopardName = leopardProfile.LeopardName,
                Weight = leopardProfile.Weight,
                Characteristics = leopardProfile.Characteristics,
                CareNeeds = leopardProfile.CareNeeds,
                ModifiedDate = leopardProfile.ModifiedDate
            });
        }

        public async Task<(int code, IQueryable<LeopardProfileView> item)> GetAllQueryable()
        {
            var result = await _repo.GetAllQueryable();

            if (result == null)
                return (404, null);

            var mapped = result.Select(x => new LeopardProfileView
            {
                LeopardProfileId = x.LeopardProfileId,
                LeopardTypeId = x.LeopardTypeId,
                LeopardName = x.LeopardName,
                Weight = x.Weight,
                Characteristics = x.Characteristics,
                CareNeeds = x.CareNeeds,
                ModifiedDate = x.ModifiedDate
            }).AsQueryable();

            return (200, mapped);
        }

        public async Task<(int code, List<LeopardProfileView> item)> GetAll()
        {
            var result = await _repo.GetAll();

            if (result.Count < 1 || result == null)
                return (404, null);

            var mapped = result.Select(x => new LeopardProfileView
            {
                LeopardProfileId = x.LeopardProfileId,
                LeopardTypeId = x.LeopardTypeId,
                LeopardName = x.LeopardName,
                Weight = x.Weight,
                Characteristics = x.Characteristics,
                CareNeeds = x.CareNeeds,
                ModifiedDate = x.ModifiedDate
            }).ToList();

            return (200, mapped);
        }

        public async Task<int> Delete(int id)
        {
            var existing = await _repo.GetById(id);

            if (existing == null)
                return 404;

            var result = await _repo.Delete(existing);

            if (result < 1)
                return 500;
            return 200;
        }

        public async Task<(int code, LeopardProfileView item)> GetById(int id)
        {
            var result = await _repo.GetById(id);

            if (result == null)
                return (404, null);

            var mapped = new LeopardProfileView
            {
                LeopardProfileId = result.LeopardProfileId,
                LeopardTypeId = result.LeopardTypeId,
                LeopardName = result.LeopardName,
                Weight = result.Weight,
                Characteristics = result.Characteristics,
                CareNeeds = result.CareNeeds,
                ModifiedDate = result.ModifiedDate
            };

            return (200, mapped);
        }

        public async Task<(int code, LeopardProfileView item)> Update(int id, LeopardProfileUpdate obj)
        {
            var existing = await _repo.GetById(id);

            if (existing == null)
                return (404, null);

            if (obj.LeopardTypeId != 0 && obj.LeopardTypeId != null)
                existing.LeopardTypeId = obj.LeopardTypeId.Value;
            existing.LeopardName = obj.LeopardName ?? existing.LeopardName;
            if (obj.Weight != 0 && obj.Weight != null)
                existing.Weight = obj.Weight.Value;
            existing.Characteristics = obj.Characteristics ?? existing.Characteristics;
            existing.CareNeeds = obj.CareNeeds ?? existing.CareNeeds;
            if (obj.ModifiedDate != existing.ModifiedDate && obj.ModifiedDate != null)
                existing.ModifiedDate = obj.ModifiedDate.Value;

            var result = await _repo.Update(existing);

            if (result < 1)
                return (500, null);

            var updated = await _repo.GetById(id);
            return (200, new LeopardProfileView
            {
                LeopardProfileId = updated.LeopardProfileId,
                LeopardTypeId = updated.LeopardTypeId,
                LeopardName = updated.LeopardName,
                Weight = updated.Weight,
                Characteristics = updated.Characteristics,
                CareNeeds = updated.CareNeeds,
                ModifiedDate = updated.ModifiedDate
            });
        }
    }
}
