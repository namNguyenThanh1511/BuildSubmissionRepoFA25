using AutoMapper;
using DAL.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Utils
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<LeopardProfile, LeopardProfileViewDTO>().ReverseMap();
            CreateMap<LeopardProfile,LeopardProfileModifyDTO>().ReverseMap();
        }
    }
}
