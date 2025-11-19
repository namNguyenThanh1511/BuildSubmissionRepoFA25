using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface ILeopardAccountRepo
    {
        Task<LeopardAccount> LoginAsync(string email, string password);
    }
}
