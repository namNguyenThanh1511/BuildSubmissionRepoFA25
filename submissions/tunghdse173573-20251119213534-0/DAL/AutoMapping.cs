using AutoMapper;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<LeopardProfile, LeopardProfileViewModel>().ForMember(dest => dest.LeopardTypeName, opt => opt.MapFrom(src => src.LeopardType.LeopardTypeName)).ReverseMap();
            CreateMap<LeopardProfile, LeopardProfileDTo>().ReverseMap();
        }
        
    }
}
