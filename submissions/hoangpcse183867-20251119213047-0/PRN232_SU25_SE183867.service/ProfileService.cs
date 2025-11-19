using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PRN232_SU25_SE183867.repository;
using PRN232_SU25_SE183867.repository.Entities;
using PRN232_SU25_SE183867.service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE183867.service
{
    public class ProfileService
    {
        private readonly ProfileRepository _repo;
        private readonly TypeRepository _typeRepo;
        private readonly ILogger<ProfileService> _logger;



        public ProfileService(ProfileRepository repo, TypeRepository typeRepo, ILogger<ProfileService> logger)
        {
            _repo = repo;
            _typeRepo = typeRepo;
            _logger = logger;
        }


        public async Task<IList<LeopardProfile>> GetAllAsync()
        {
            var profileList = await _repo.GetAllAsync(query => query.Include(p => p.LeopardType));
            if (profileList.IsNullOrEmpty()) throw new NullReferenceException("Resource not found");
      
            return profileList;
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            var profile = await _repo.FindAsync(p => p.LeopardProfileId == id, "LeopardType");
            if (profile == null) throw new NullReferenceException();
            return profile;
        }


        public async Task CreateProfile(CreateProfileReq request)
        {
            try
            {
                var type = await _typeRepo.GetByIdAsync(request.LeopardTypeId);
                if (type == null) throw new NullReferenceException();
                var oldProfile = await _repo.GetByIdAsync(request.LeopardProfileId);
                if (oldProfile != null) throw new ArgumentException("LeopardProfileId is already have");

                var profile = new LeopardProfile
                {
                    LeopardTypeId = request.LeopardTypeId,
                    LeopardName = request.LeopardName,
                    Weight = request.Weight,
                    CareNeeds = request.CareNeeds,
                    Characteristics = request.Characteristics,
                    ModifiedDate = request.ModifiedDate
                };


                await _repo.InsertAsync(profile);
                await _repo.SaveAsync();
            } catch (Exception e)
            {
                _logger.LogError("Error at creaet  asue by {}", e.Message);
            }
        }


        public async Task DeleteProfile(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveAsync();
        }


        public async Task UpdateProfile(int id, CreateProfileReq request)
        {
            try
            {
                var profile = await _repo.GetByIdAsync(id);
                if (profile == null) throw new NullReferenceException();
                var type = await _typeRepo.GetByIdAsync(request.LeopardTypeId);
                if (type == null) throw new NullReferenceException();


                profile.LeopardName = request.LeopardName;
                profile.Weight = request.Weight;
                profile.CareNeeds = request.CareNeeds;
                profile.Characteristics = request.Characteristics;
                profile.ModifiedDate = request.ModifiedDate;


                await _repo.UpdateAsync(profile);
                await _repo.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error at update  asue by {}", e.Message);
            }
        }

    }
}
