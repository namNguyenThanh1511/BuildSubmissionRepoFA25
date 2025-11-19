using BusinessObjects;
using Repositories;

namespace Services
{
    public interface ILeopardTypeService
    {
        List<LeopardType> GetAllLeopardTypes();
        LeopardType GetLeopardTypeById(int? id);
    }
}
