using PRN232_SU25_SE183867.repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE183867.repository
{
    public class AuthRepository : GenericRepository<LeopardAccount>
    {

        public AuthRepository(Dbcontext context) : base(context)
        {

        }
    }
}
