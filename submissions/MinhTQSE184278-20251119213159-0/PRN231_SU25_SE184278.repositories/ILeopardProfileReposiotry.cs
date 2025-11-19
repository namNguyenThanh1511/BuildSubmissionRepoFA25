using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE184278.repositories
{
    public interface ILeopardProfileReposiotry
    {
        IQueryable<LeopardProfile> Query();



        Task<LeopardProfile?> GetByIdAsync(int id);



       Task CreateAsync(LeopardProfile l);



        Task UpdateAsync(LeopardProfile l);


        Task DeleteAsync(LeopardProfile l);


        Task<int> GetMaxIdAsync();
 
    }
}
