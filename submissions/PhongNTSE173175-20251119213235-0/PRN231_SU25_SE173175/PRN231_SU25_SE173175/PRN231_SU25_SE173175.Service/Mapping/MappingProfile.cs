using AutoMapper;
using Microsoft.Identity.Client;
using PRN231_SU25_SE173175.Repository.Entities;
using PRN231_SU25_SE173175.Service.DTOs;

namespace PRN231_SU25_SE173175.Service.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<LeopardProfile, LeopardProfileResponse>()
				.ForMember(dest => dest.LeopardType, opt => opt.MapFrom(src => src.LeopardType))
				.ReverseMap();


			CreateMap<LeopardType, LeopardTypeResponse>()
				.ReverseMap();
			
		}
	}
}
