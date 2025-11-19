using BusinessObjects;
using DataTransferObjects;
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
        private readonly ILeopardProfileRepository _Repository;

        public LeopardProfileService(ILeopardProfileRepository repository)
        {
            this._Repository = repository;
        }

        public List<LeopardProfile> GetLeopardProfile()
        {
            return _Repository.GetLeopardProfile();
        }

        public LeopardProfile GetLeopardProfileById(int id)
        {
            return _Repository.GetLeopardProfileById(id);
        }

        public void AddLeopardProfile(LeopardProfileDTO item)
        {
            _Repository.AddLeopardProfile(item);
        }

        public void UpdateLeopardProfile(UpdateLeopardProfileDTO item)
        {
            _Repository.UpdateLeopardProfile(item);
        }

        public void RemoveLeopardProfile(int id)
        {
            _Repository.DeleteLeopardProfile(id);
        }
    }
}
