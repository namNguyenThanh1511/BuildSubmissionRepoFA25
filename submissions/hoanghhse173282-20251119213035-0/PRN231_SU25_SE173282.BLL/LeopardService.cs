using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE173282.DAL;
using PRN231_SU25_SE173282.DAL.Model;

namespace PRN231_SU25_SE173282.BLL
{
    public class LeopardService
    {
        private readonly LeopardRepository _repository;

        public LeopardService() => _repository = new LeopardRepository();

        public async Task<int> Create(LeopardCreateModel leopard)
        {

            var leopardProfileId = await _repository.GetMaxIdAsync() + 1;

            return await _repository.CreateAsync(new DAL.Model.LeopardProfile
            {
                LeopardProfileId = leopardProfileId,
                LeopardTypeId = leopard.LeopardTypeId,
                LeopardName = leopard.LeopardName,
                Weight = leopard.Weight,
                Characteristics = leopard.Characteristics,
                CareNeeds = leopard.CareNeeds,
            });
        }

        public async Task<int> Delete(int id)
        {
            var item = await _repository.GetHandbagById(id);

            if (item == null)
            {
                return 2;
            }

            return await _repository.RemoveAsync(item) ? 1 : 0;

        }

        public async Task<LeopardProfile> GetHandbagById(int id)
        {
            return await _repository.GetHandbagById(id);
        }

        public async Task<List<LeopardProfile>> GetHandbags()
        {
            return await _repository.GetHandbags();
        }

        public async Task<int> Update(LeopardUpdateModel leopard)
        {
            var item = await _repository.GetHandbagById(leopard.LeopardProfileId);

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
