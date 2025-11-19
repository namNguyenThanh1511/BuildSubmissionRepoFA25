using PRN232_SU25_SE181544.BLL.DTOs;
using PRN232_SU25_SE181544.DAL.Models;
using PRN232_SU25_SE181544.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE181544.BLL.Services
{
    public class LeopardProfileService
    {
        LeopardProfileRepository repository = new();
        public async Task<List<LeopardType>> GetAllBrands()
        {
            return await repository.GetAllBrand();
        }

        public async Task<LeopardType> GetbrandById(int id)
        {
            return await repository.GetBrandById(id);

        }


        public async Task<List<LeopardProfile>> GetAll()
        {
            return await repository.GetAll();
        }
        public async Task<LeopardProfile> GetById(int id)
        {
            return await repository.GetById(id);
        }
        public async Task<LeopardProfile> Add(LeopardRequestDto dto)
        {

            LeopardProfile product = new LeopardProfile
            {
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,
            };

            var brandExists = await repository.GetBrandById(dto.LeopardTypeId);
            if (brandExists == null)
            {
                return null;
            }

            var productExists = await repository.GetById(product.LeopardProfileId);

            if (productExists != null)
            {
                return null;
            }
            return await repository.Add(product);
        }
        public async Task<LeopardProfile> Update(LeopardRequestDto dto, int id)
        {
            var productExists = await repository.GetById(id);

            if (productExists == null)
            {
                return null;
            }

            var brandExist = await repository.GetBrandById(dto.LeopardTypeId);
            if (brandExist == null)
            {
                return null;
            }

            productExists.LeopardTypeId = dto.LeopardTypeId;
            productExists.LeopardName = dto.LeopardName;
            productExists.Weight = dto.Weight;
            productExists.Characteristics = dto.Characteristics;
            productExists.CareNeeds = dto.CareNeeds;
            productExists.ModifiedDate = dto.ModifiedDate;

            return await repository.Update(productExists);
        }
        public async Task<LeopardProfile> Delete(int id)
        {
            LeopardProfile productExists = await repository.GetById(id);
            if (productExists == null)
            {
                return null;
            }
            return await repository.Delete(productExists);
        }
        public async Task<Object> Search(string leopardName, double weight)
        {

            var handbags = await repository.Search(leopardName, weight);

            return handbags;
        }
    }
}
