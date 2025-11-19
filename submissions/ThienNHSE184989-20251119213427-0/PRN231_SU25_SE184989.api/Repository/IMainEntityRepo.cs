using Repository.Base;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IMainEntityRepo : IGenericRepository<LeopardProfile>
    {

    }
    public class MainEntityRepo : GenericRepository<LeopardProfile>, IMainEntityRepo
    {
        public MainEntityRepo(SU25LeopardDBContext context) : base(context) { }
    }

}
