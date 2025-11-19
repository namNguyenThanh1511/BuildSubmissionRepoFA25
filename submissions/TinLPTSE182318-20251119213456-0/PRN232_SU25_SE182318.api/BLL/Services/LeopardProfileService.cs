using BLL.DTOs;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly IGenericRepo<LeopardProfile> _repo;
        private readonly ILeopardProfileRepo _leopardProfileRepo;

        public LeopardProfileService(IGenericRepo<LeopardProfile> repo, ILeopardProfileRepo leopardProfileRepo)
        {
            _repo = repo;
            _leopardProfileRepo = leopardProfileRepo;
        }

        public async Task<LeopardProfile> CreateLeopardProfile(LeopardProfileDTO dto)
        {
            var pattern = "^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (!Regex.IsMatch(dto.LeopardName, pattern))
            {
                throw new CustomException("HB40001", "Invalid leopard name", HttpStatusCode.BadRequest);
            }

            if (dto.Weight <= 15)
            {
                throw new CustomException("HB40001", "Invalid weight", HttpStatusCode.BadRequest);
            }
            try
            {
                var profile = new LeopardProfile
                {
                    LeopardTypeId = dto.LeopardTypeId,
                    LeopardName = dto.LeopardName,
                    Weight = dto.Weight,
                    Characteristics = dto.Characteristics,
                    CareNeeds = dto.CareNeeds,
                    ModifiedDate = dto.ModifiedDate
                };
                await _repo.AddAsync(profile);
                return profile;
            }
            catch (Exception ex)
            {
                throw new CustomException("HB50001", "Failed to create new profile", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> DeleteLeopardProfile(int id)
        {
            var profile = await _repo.GetByIdAsync(id);
            if (profile != null)
            {
                try
                {
                    await _repo.DeleteAsync(profile);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new CustomException("HB50001", "Failed to delete profile", HttpStatusCode.InternalServerError);
                }
            }
            else return false;
        }

        public async Task<LeopardProfile> GetLeopardProfileById(int id)
        {
            var profile = await _leopardProfileRepo.GetLeopardProfileById(id);
            return profile;
        }

        public async Task<List<LeopardProfile>> GetLeopardProfiles()
        {
            var profileList = await _leopardProfileRepo.GetLeopardProfiles();
            return profileList;
        }

        public async Task<List<LeopardProfile>> SearchLeopardProfile(string leopardName, double weight)
        {
            var profileList = await _leopardProfileRepo.SearchLeopard(leopardName, weight);
            return profileList;
        }

        public async Task<bool> UpdateLeopardProfile(int id, LeopardProfileUpdateDTO dto)
        {
            var pattern = "^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (!Regex.IsMatch(dto.LeopardName, pattern))
            {
                throw new CustomException("HB40001", "Invalid leopard name", HttpStatusCode.BadRequest);
            }

            if (dto.Weight <= 15)
            {
                throw new CustomException("HB40001", "Invalid weight", HttpStatusCode.BadRequest);
            }

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
            {
                return false;
            }

            try
            {
                existing.LeopardTypeId = dto.LeopardTypeId;
                existing.LeopardName = dto.LeopardName;
                existing.Weight = dto.Weight;
                existing.Characteristics = dto.Characteristics;
                existing.CareNeeds = dto.CareNeeds;
                existing.ModifiedDate = dto.ModifiedDate;
                await _repo.UpdateAsync(existing);
                return true;
            }
            catch (Exception ex)
            {
                throw new CustomException("HB50001", "Failed to update profile", HttpStatusCode.InternalServerError);
            }
        }
    }
}
