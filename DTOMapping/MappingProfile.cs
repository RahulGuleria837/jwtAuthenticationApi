using AutoMapper;
using JWTAuthentication.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDTO, ApplicationUser>().ReverseMap();

        }

    }
}
