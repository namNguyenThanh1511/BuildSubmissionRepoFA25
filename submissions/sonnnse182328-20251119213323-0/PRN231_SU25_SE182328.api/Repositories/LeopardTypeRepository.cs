using BusinessObjects;
using DAO;

namespace Repositories
{
    public class LeopardTypeRepository : ILeopardTypeRepository
    {
        public List<LeopardType> GetAllLeopardTypes()
            => LeopardTypeDAO.Instance.GetLeopardTypes();

        public LeopardType GetLeopardTypeById(int? id)
            => LeopardTypeDAO.Instance.GetLeopardTypeById(id);
    }
}
