using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILeopardTypeService
    {
        Task<List<LeopardType>> GetAllAsync();
    }
}
