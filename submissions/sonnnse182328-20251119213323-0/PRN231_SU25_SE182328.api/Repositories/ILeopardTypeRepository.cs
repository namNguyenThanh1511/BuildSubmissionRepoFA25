using BusinessObjects;

namespace Repositories
{
    public interface ILeopardTypeRepository
    {
        List<LeopardType> GetAllLeopardTypes();
        LeopardType GetLeopardTypeById(int? id);
    }
}
