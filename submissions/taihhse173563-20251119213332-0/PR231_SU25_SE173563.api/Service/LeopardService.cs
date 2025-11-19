using Repository;
using Repository.Models;
using Service.Model;
using System.Text.RegularExpressions;

namespace Service
{
    public class LeopardService
    {
        private readonly UnitOfWork _unitOfWork;
        public LeopardService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<List<LeopardProfile>> GetAllLeoAsync()
        {
            return await _unitOfWork.GetRepository<LeopardProfile>().GetAllByPropertyAsync(null, "LeopardType");
        }
        public async Task<LeopardProfile?> GetLeoByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(h => h.LeopardProfileId == id, true, "LeopardType");
        }
 
        public async Task AddLeoAsync(LeopardCreateDTO dto)
        {

            string regexPattern = @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (!Regex.IsMatch(dto.LeopardName, regexPattern))
            {
                throw new ArgumentException();

            }
            if (dto.Weight <= 15)
            {
                throw new ArgumentException();
            }
            LeopardProfile leo = new();
            leo.LeopardTypeId = dto.LeopardTypeId;
            leo.LeopardName = dto.LeopardName;
            leo.Weight = dto.Weight;
            leo.Characteristics = dto.Characteristics;
            leo.CareNeeds = dto.CareNeeds;
            leo.ModifiedDate = dto.ModifiedDate;

            await _unitOfWork.GetRepository<LeopardProfile>().AddAsync(leo);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateLeoAsync(int id, LeopardCreateDTO dto)
        {
            LeopardProfile? leo = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(h => h.LeopardProfileId == id, true, "LeopardType");
            if (leo == null)
            {
                throw new ArgumentNullException();
            }
            string regexPattern = @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$";
            if (!Regex.IsMatch(dto.LeopardName, regexPattern))
            {
                throw new ArgumentException();

            }
            if (dto.Weight <= 15)
            {
                throw new ArgumentException();
            }

            leo.LeopardTypeId = dto.LeopardTypeId;
            leo.LeopardName = dto.LeopardName;
            leo.Weight = dto.Weight;
            leo.Characteristics = dto.Characteristics;
            leo.CareNeeds = dto.CareNeeds;
            leo.ModifiedDate = dto.ModifiedDate;

            await _unitOfWork.GetRepository<LeopardProfile>().UpdateAsync(leo);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteLeoAsync(int id)
        {
            LeopardProfile? leo = await _unitOfWork.GetRepository<LeopardProfile>().GetByPropertyAsync(h => h.LeopardProfileId == id, true, "LeopardType");
            if (leo == null)
            {
                throw new ArgumentNullException();
            }
            await _unitOfWork.GetRepository<LeopardProfile>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IQueryable<LeopardProfile>> Search(string? leopardName, double? weight)
        {
           return await _unitOfWork.GetRepository<LeopardProfile>()
                .GetQueryAsync(x => (x.LeopardName.Contains(leopardName) || string.IsNullOrWhiteSpace(leopardName))
                                            && (x.Weight == weight|| weight == null),
                                           includeProperties : "LeopardType");

        }
    }
    
}
