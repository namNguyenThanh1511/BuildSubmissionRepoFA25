using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicLayer.Service
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private static readonly Regex _modelNameRegex =
            new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
        // WEIGHT > 15

        private readonly ILeopardProfileRepository _repo;
        
        public LeopardProfileService(ILeopardProfileRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<LeopardProfile> GetAllProfiles()
        {
            return _repo.GetAll();
        }

        public LeopardProfile GetProfileById(int profileId)
        {
            var h = _repo.GetById(profileId);
            if (h == null)
                throw new ServiceException("HB40401", "Resource not found");
            return h;
        }

        public LeopardProfile CreateProfile(LeopardProfile profile)
        {
            //kiem tra dau vao
            Validate(profile);
            return _repo.Create(profile);
        }

        public LeopardProfile UpdateProfile(LeopardProfile profile)
        {
            if (_repo.GetById(profile.LeopardProfileId) == null)
                throw new ServiceException("HB40401", "Resource not found");

            Validate(profile);
            return _repo.Update(profile);
        }

        public void DeleteProfile(int profileId)
        {
            if (_repo.GetById(profileId) == null)
                throw new ServiceException("HB40401", "Resource not found");

            _repo.Delete(profileId);
        }

        public IQueryable<LeopardProfile> SearchProfile(string leopardName)
        {
            return _repo.Search(leopardName);
        }

        private void Validate(LeopardProfile validate)
        {
            if (string.IsNullOrWhiteSpace(validate.LeopardName)
             || !_modelNameRegex.IsMatch(validate.LeopardName))
                throw new ServiceException("HB40001", "Missing/invalid input");

            if (validate.Weight <= 15)
                throw new ServiceException("HB40001", "Missing/invalid input");
        }

        public class ServiceException : Exception
        {
            public string ErrorCode { get; }
            public ServiceException(string errorCode, string message)
                : base(message)
                => ErrorCode = errorCode;
        }

    }
}
