using PRN231_SU25_SE173362.DAL;
using PRN231_SU25_SE173362.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.BLL
{
    public class LeopardProfileService
    {
        private readonly LeopardProfileRepository _repository;

        public LeopardProfileService() => _repository = new LeopardProfileRepository();

        public async Task<int> Create(LeopardCreateModel leopard)
        {
           

            return await _repository.CreateAsync(new LeopardProfile
            {
                LeopardProfileId = leopard.LeopardProfileId,
                LeopardTypeId = leopard.LeopardTypeId,
                LeopardName = leopard.LeopardName,
                Weight = leopard.Weight,
                Characteristics = leopard.Characteristics,
                CareNeeds = leopard.CareNeeds,
                ModifiedDate = leopard.ModifiedDate
            });

            
        }

        public async Task<int> Delete(int id)
        {
            var item = await _repository.GetleopardById(id);

            if (item == null)
            {
                return 2;
            }

            return await _repository.RemoveAsync(item) ? 1 : 0;

        }

        public async Task<LeopardProfile> GetLeopardById(int id)
        {
            return await _repository.GetleopardById(id);
        }

        public async Task<List<LeopardProfile>> GetLeopards()
        {
            return await _repository.Getleopards();
        }

        public async Task<int> Update(LeopardUpdateModel leopard)
        {
            var item = await _repository.GetleopardById(leopard.LeopardProfileId);

            if (item == null)
            {
                return 2;
            }

            

            item.LeopardName = leopard.LeopardName != null ? leopard.LeopardName : item.LeopardName;
            item.Weight = leopard.Weight != null ? leopard.Weight : item.Weight;
            item.Characteristics = leopard.Characteristics != null ? leopard.Characteristics : item.Characteristics;
            item.CareNeeds = leopard.CareNeeds != null ? leopard.CareNeeds : item.CareNeeds;
            item.LeopardTypeId = leopard.LeopardTypeId != null ? leopard.LeopardTypeId : item.LeopardTypeId;
            item.ModifiedDate = leopard.ModifiedDate != null ? leopard.ModifiedDate : item.ModifiedDate;


            
            return await _repository.UpdateAsync(item);

        }

    }
}
