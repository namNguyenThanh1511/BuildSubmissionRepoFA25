using AutoMapper;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class LeopardProfileService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeopardProfileService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateNewLeopardProfile(LeopardProfileDTo leopardProfileDTO)
        {
            
            var leopard = _mapper.Map<LeopardProfile>(leopardProfileDTO);

            await _unitOfWork.GetRepository<LeopardProfile>().AddAsync(leopard);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteLeopardProfile(int id)
        {
            var leopardProfile = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(hb => hb.LeopardProfileId == id);
            await _unitOfWork.GetRepository<LeopardProfile>().DeleteAsync(leopardProfile.LeopardProfileId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<LeopardProfileViewModel>> GetAllLeopards()
        {
            var leopardProfiles = await _unitOfWork.GetRepository<LeopardProfile>().GetAllByPropertyAsync();
            var result = _mapper.Map<List<LeopardProfileViewModel>>(leopardProfiles);
            return result;
        }

        public async Task<LeopardProfileDTo> GetLeopardProfile(int id)
        {
            var leopardProfile = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(lp => lp.LeopardProfileId == id);
            if (leopardProfile == null) return null;
            var result = _mapper.Map<LeopardProfileDTo>(leopardProfile);
            return result;
        }

        public async Task<List<LeopardProfileViewModel>> SearchLeopardProfile(string? cheetarName, double? weight)
        {

            var searchlist = await _unitOfWork.GetRepository<LeopardProfile>().GetAllByPropertyAsync(
                h => (h.LeopardName.Contains(cheetarName) || string.IsNullOrWhiteSpace(cheetarName)) && (h.Weight == weight), includeProperties: "LeopardType");
            var searchresult = _mapper.Map<List<LeopardProfileViewModel>>(searchlist);
            if (searchresult == null) return null;
            return searchresult;
        }

        public async Task UpdateLeopardProfile(int id, LeopardProfileDTo leopardProfileDTo)
        {
            var leopard = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(b => b.LeopardProfileId == id);
            if (leopard == null) throw new Exception("LeopardProfile not found");
            _mapper.Map(leopardProfileDTo, leopard);
            await _unitOfWork.GetRepository<LeopardProfile>().UpdateAsync(leopard);
            await _unitOfWork.SaveAsync();

        }
        public bool ValidateProduct(LeopardProfileDTo leopardProfileDTo)
        {

            // Regex rule for modelName
            string pattern = @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (string.IsNullOrWhiteSpace(leopardProfileDTo.LeopardName) || !Regex.IsMatch(leopardProfileDTo.LeopardName, pattern))
            {
                return false;
            }

            // Price > 0
            if (leopardProfileDTo.Weight <= 15)
            {
                return false;
            }


            return true;
        }
    }
}
