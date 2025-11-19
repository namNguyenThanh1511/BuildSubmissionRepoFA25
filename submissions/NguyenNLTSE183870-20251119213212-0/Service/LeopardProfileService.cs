using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfile>?> GetAll();
        Task<LeopardProfile?> GetById(int id);
        Task<int> Create(CreateRequest item);
        Task<int> Update(int id, CreateRequest item);
        Task<bool> Delete(int id);
        IQueryable<LeopardProfile> SearchOData();
    }
    public class LeopardProfileService: ILeopardProfileService
    {
        private readonly LeopardProfileRepository _repo;
        public LeopardProfileService(LeopardProfileRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<int> Create(CreateRequest request)
        {

            var dto = new LeopardProfile
            {
               
                LeopardTypeId= request.LeopardTypeId,
                LeopardName = request.LeopardName,
                Weight = request.Weight,
                Characteristics = request.Characteristics,
                CareNeeds  = request.CareNeeds,
                ModifiedDate = request.ModifiedDate,
            };


            return await _repo.CreateAsync(dto);
        }
        public async Task<bool> Delete(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($" not found.");

            }
            return await _repo.RemoveAsync(item);
        }

        public async Task<List<LeopardProfile>?> GetAll()
        {
            return await _repo.GetAllInclude();
        }
        public async Task<LeopardProfile?> GetById(int id)
        {
            return await _repo.GetByIdAsync2(id);
        }



        public async Task<int> Update(int id, CreateRequest request)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"not found.");
            }

          item.Weight = request.Weight;
            item.Characteristics = request.Characteristics;
            item.CareNeeds = request.CareNeeds;
            item.LeopardName= request.LeopardName;
            item.LeopardTypeId= request.LeopardTypeId;
            item.ModifiedDate = request.ModifiedDate;


            return await _repo.UpdateAsync(item);
        }
        public IQueryable<LeopardProfile> SearchOData()
        {
            return _repo.Query().Include(h => h.LeopardType).AsQueryable();
        }
    }
}
