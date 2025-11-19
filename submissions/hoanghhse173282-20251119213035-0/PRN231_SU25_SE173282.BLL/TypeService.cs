using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE173282.DAL;
using PRN231_SU25_SE173282.DAL.Model;

namespace PRN231_SU25_SE173282.BLL
{
    public class TypeService
    {
        private readonly TypeRepository _repository;

        public TypeService() => _repository ??= new TypeRepository();

        public async Task<List<LeopardType>> GetBrands()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LeopardType> GetBrandById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
