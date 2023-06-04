using AutoMapper;
using Demo.DAL.Models;
using session3Mvc.ViewModels;

namespace session3Mvc.Mapping_Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();


        }
    }
}
    