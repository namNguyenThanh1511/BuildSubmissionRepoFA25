using Repository.Entities;

namespace Repository;

public class AuthRepository : GenericRepository<LeopardAccount>
{
    public AuthRepository(Dbcontext context) : base(context)
    {
    }
}