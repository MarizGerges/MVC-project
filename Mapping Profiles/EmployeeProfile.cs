using AutoMapper;
using Demo.DAL.Models;
using session3Mvc.ViewModels;

namespace session3Mvc.Mapping_Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();  
        }
    } 
}
