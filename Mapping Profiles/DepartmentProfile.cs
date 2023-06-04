using AutoMapper;
using Demo.DAL.Models;
using session3Mvc.ViewModels;

namespace session3Mvc.Mapping_Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
