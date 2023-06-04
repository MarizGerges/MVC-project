using AutoMapper;
using Microsoft.AspNetCore.Identity;
using session3Mvc.ViewModels;

namespace session3Mvc.Mapping_Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
         CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(D=>D.RoleName,o=>o.MapFrom(s=>s.Name))
                .ReverseMap();

        }
    }
}
