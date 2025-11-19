using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class LeopardProfileRepository : GenericRepository<LeopardProfile>
	{
		public LeopardProfileRepository(Dbcontext context) : base(context)
		{
		}
	}
}
