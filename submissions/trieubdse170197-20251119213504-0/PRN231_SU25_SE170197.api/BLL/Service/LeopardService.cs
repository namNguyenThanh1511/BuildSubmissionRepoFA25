using AutoMapper;
using BLL.Interface;
using DAL;
using Data.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class LeopardService : ILeopardService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeopardService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<LeopardViewModel>> GetAllLeopard()
        {
            var leopard = await _unitOfWork.GetRepository<LeopardProfile>().GetAllByPropertyAsync();
            var leopardViewModels = _mapper.Map<List<LeopardViewModel>>(leopard);
            return leopardViewModels;
        }

        public async Task<LeopardViewModel> GetLeopardById(int id)
        {
            var leopard = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(
                h => h.LeopardProfileId == id
            );
            if (leopard == null) return null;
            var result = _mapper.Map<LeopardViewModel>(leopard);
            return result;
        }

        public async Task DeleteLeopard(int id)
        {
            var leopard = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(
                h => h.LeopardProfileId == id
            );
            if (leopard == null)
            {
                throw new ArgumentException($"Leopard with ID {id} not found");
            }

            await _unitOfWork.GetRepository<LeopardProfile>().DeleteAsync(leopard.LeopardProfileId);
            await _unitOfWork.SaveAsync();
        }
    }
}
