using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repo;
        public ProfileService(IProfileRepository repo)
        {
            _repo = repo;
        }

      
        public async Task<IEnumerable<LeopardProfile>> GetAllItemsAsync()
        {
            return await _repo.FindAll(null, i => i.LeopardType).ToListAsync();
        }
        public async Task<LeopardProfile?> GetItemByIdAsync(int id)
        {
            return await _repo.FindById(id);
        }

        public async Task<LeopardProfile> AddItemAsync(ProfileRequestDto item)
        {
            var newItem = new LeopardProfile
            {
               LeopardTypeId = item.LeopardTypeId,
               LeopardName = item.LeopardName,
               Weight = item.Weight,
               Characteristics = item.Characteristics,
               CareNeeds = item.CareNeeds,
               ModifiedDate = item.ModifiedDate,
            };
           return await _repo.CreateItem(newItem);

        }

        public async Task<LeopardProfile> UpdateItemAsync(int id, ProfileRequestDto item)
        {
            var existItem = await _repo.FindById(id);
            if (existItem == null)
                throw new KeyNotFoundException("Not found");
            existItem.LeopardTypeId = item.LeopardTypeId;
            existItem.LeopardName = item.LeopardName;
            existItem.Weight = item.Weight;
            existItem.Characteristics = item.Characteristics;
            existItem.CareNeeds = item.CareNeeds;
            existItem.ModifiedDate = item.ModifiedDate;

           return await _repo.UpdateItem(existItem);
        }

        public async Task DeleteItemAsync(int id)
        {
            var existItem = await _repo.FindById(id);
            if (existItem == null)
                throw new KeyNotFoundException("Not found");
            await _repo.Delete(existItem);
        }
    }
}
