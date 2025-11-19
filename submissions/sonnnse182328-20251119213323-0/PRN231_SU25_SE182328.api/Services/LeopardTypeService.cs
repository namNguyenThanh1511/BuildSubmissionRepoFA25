using BusinessObjects;
using Repositories;

namespace Services
{
    public class LeopardTypeService : ILeopardTypeService
    {
        private readonly ILeopardTypeRepository _LeopardTypeRepository;
        public LeopardTypeService(ILeopardTypeRepository LeopardTypeRepository)
        {
            _LeopardTypeRepository = LeopardTypeRepository;
        }
        public List<LeopardType> GetAllLeopardTypes()
        {
            return _LeopardTypeRepository.GetAllLeopardTypes();
        }

        public LeopardType GetLeopardTypeById(int? id)
        {
            return _LeopardTypeRepository.GetLeopardTypeById(id);
        }
    }
}
