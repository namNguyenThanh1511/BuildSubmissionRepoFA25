using BusinessObjects;
using DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repository;

        public LeopardProfileService(ILeopardProfileRepository repository)
        {
            this._repository = repository;
        }

        public List<LeopardProfile> GetLeopardProfiles()
        {
            return _repository.GetLeopardProfiles();
        }

        public LeopardProfile GetLeopardProfileById(int id)
        {
            return _repository.GetLeopardProfileById(id);
        }

        public void CreateLeopardProfile(LeopardProfileDTO item)
        {
            _repository.AddLeopardProfile(item);
        }

        public void UpdateLeopardProfile(LeopardProfileDTO item)
        {
            _repository.UpdateLeopardProfile(item);
        }

        public void RemoveLeopardProfile(int id)
        {
            _repository.DeleteLeopardProfile(id);
        }

        public List<object> SearchLeopardProfiles(string? LeopardName, double? Weight)
        {
            return _repository.SearchLeopardProfiles(LeopardName, Weight);
        }
    }
}
