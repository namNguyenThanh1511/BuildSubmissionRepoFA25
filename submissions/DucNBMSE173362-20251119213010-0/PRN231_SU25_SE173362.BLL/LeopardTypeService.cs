using PRN231_SU25_SE173362.DAL;
using PRN231_SU25_SE173362.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173362.BLL
{
    public class LeopardTypeService
    {
        
            private readonly LeopardTypeRepository _repository;

            public LeopardTypeService() => _repository ??= new LeopardTypeRepository();

            

            public async Task<LeopardType> GetTypeById(int id)
            {
                return await _repository.GetTypeById(id);
            }
        
    }
}
