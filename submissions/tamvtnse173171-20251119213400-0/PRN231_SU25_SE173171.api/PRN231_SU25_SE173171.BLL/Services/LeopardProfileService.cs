using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173171.BLL.DTOs;
using PRN231_SU25_SE173171.BLL.Interfaces;
using PRN231_SU25_SE173171.DAL.Entities;
using PRN231_SU25_SE173171.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.Services
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly ILeopardProfileRepository _repo;

        public LeopardProfileService(ILeopardProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<LeopardProfile>> GetAllList()
        {
            return await _repo.GetAll();
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task Create(CreateLeopardProfileRequest request)
        {
            var profile = new LeopardProfile();

            profile.LeopardProfileId = request.LeopardProfileId;
            profile.LeopardTypeId = request.LeopardTypeId;
            profile.LeopardName = request.LeopardName;
            profile.Weight = request.Weight;
            profile.Characteristics = request.Characteristics;
            profile.CareNeeds = request.CareNeeds;
            profile.ModifiedDate = request.ModifiedDate;
            try
            {
                await _repo.Add(profile);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(int id, UpdateLeopardProfileRequest request)
        {
            var profile = await _repo.GetById(id);
            if (profile == null)
            {
                throw new Exception($"Profile with id: {id} is not found");
            }
            profile.LeopardTypeId = request.LeopardTypeId;
            profile.LeopardName = request.LeopardName;
            profile.Weight = request.Weight;
            profile.Characteristics = request.Characteristics;
            profile.CareNeeds = request.CareNeeds;
            profile.ModifiedDate = request.ModifiedDate;

            try
            {
                await _repo.Update(profile);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var profile = await _repo.GetById(id);
                if (profile == null)
                {
                    throw new Exception($"Profile with id: {id} is not found");
                }
                else
                {
                    await _repo.Delete(id);
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}
