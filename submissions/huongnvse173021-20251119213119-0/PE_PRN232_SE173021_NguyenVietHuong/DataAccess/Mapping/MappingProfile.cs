using AutoMapper;
using DataAccess.DTOs;
using DataAccess.Entities;
using System.Collections.Generic;

namespace Repositories.Mapping
{
    public class MappingProfile : Profile
    {
       public MappingProfile() 
        {
            CreateMap<LeopardProfile, LeopardProfileDTO>()
                .ForMember(dept => dept.LeopardTypeName, opt => opt.MapFrom(src => src.LeopardType.LeopardTypeName))
                .ReverseMap();
            CreateMap<LeopardProfile, LeopardProfileUpdateDTO>().ReverseMap();
        } 
    }
}
