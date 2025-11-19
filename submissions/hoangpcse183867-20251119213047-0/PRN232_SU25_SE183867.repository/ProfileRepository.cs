using PRN232_SU25_SE183867.repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE183867.repository
{
    public class ProfileRepository : GenericRepository<LeopardProfile>
    {

        public ProfileRepository(Dbcontext context) : base(context)
        {

        }
    }
}
