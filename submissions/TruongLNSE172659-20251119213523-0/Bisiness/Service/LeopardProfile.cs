using Bisiness.Iservice;
using DataAccess.Dto;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bisiness.Service
{
    public class LeopardProfile : ILeooardProfile
    {
        private readonly LeopardProfileRepository _leopardProfileRepository;

        public LeopardProfile(LeopardProfileRepository leopardProfileRepository)
        {
            _leopardProfileRepository = leopardProfileRepository;
        }


        public async Task<BaseModel> getAllHandbags()
        {
            var temp = await _leopardProfileRepository.GellAllLeopardProfile();
            return new BaseModel
            {
                data = temp
            };

        }

        public async Task<BaseModel> GetbyId(int id)
        {
            var temp = await _leopardProfileRepository.Getbyid(id);

            return new BaseModel
            {
                data = temp
            };
        }
    }
}
