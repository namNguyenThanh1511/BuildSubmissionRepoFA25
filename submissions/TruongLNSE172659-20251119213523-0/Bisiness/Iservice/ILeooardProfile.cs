using DataAccess.Dto;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bisiness.Iservice
{
    public interface ILeooardProfile
    {
        Task<BaseModel> getAllHandbags();

        Task<BaseModel> GetbyId(int id);

    }
}
