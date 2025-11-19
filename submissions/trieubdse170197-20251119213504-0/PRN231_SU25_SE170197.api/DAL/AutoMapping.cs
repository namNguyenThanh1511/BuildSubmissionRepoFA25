using AutoMapper;
using Data.DTO;
using Data.Models;
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
            CreateMap<LeopardProfile, LeopardViewModel>().ReverseMap();
        }
    }
}
