using AutoMapper;
using BLL.Interface;
using DAL.DTO;
using DAL.Repository;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class LeopardProfileService : ILeopardProfileService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LeopardProfileService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateNew(LeopardProfileModifyDTO model)
        {
            
            var addprofile = _mapper.Map<LeopardProfile>(model);
            await _unitOfWork.GetRepository<LeopardProfile>().AddAsync(addprofile);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteProfile(int id)
        {
            var exited = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(p => p.LeopardProfileId == id);
            await _unitOfWork.GetRepository<LeopardProfile>().DeleteAsync(exited.LeopardTypeId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<LeopardProfileViewDTO>> GetAllProfile()
        {
            var list = await _unitOfWork.GetRepository<LeopardProfile>().GetAllByPropertyAsync();
            var result = _mapper.Map<List<LeopardProfileViewDTO>>(list);
            return result;
        }

        public async Task<LeopardProfileViewDTO> GetProfileById(int id)
        {
            var profile = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(p => p.LeopardProfileId == id);
            var result = _mapper.Map<LeopardProfileViewDTO>(profile);
            return result;
        }

        public async Task UpdateProfile(LeopardProfileModifyDTO model, int id)
        {
            var exited = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(p => p.LeopardProfileId == id);
            _mapper.Map(model, exited);
            await _unitOfWork.GetRepository<LeopardProfile>().UpdateAsync(exited);
            await _unitOfWork.SaveAsync();
        }
        
        public bool ValidateProduct(LeopardProfileModifyDTO leopardProfileModifyDTO)
        {

            // Regex rule for modelName
            string pattern = @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (string.IsNullOrWhiteSpace(leopardProfileModifyDTO.LeopardName) || !Regex.IsMatch(leopardProfileModifyDTO.LeopardName, pattern))
            {
                return false;
            }

          
            if (leopardProfileModifyDTO.Weight <= 15)
            {
                return false;
            }

            return true;
        }

    }
}
