using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace session3Mvc.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is requierd")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "max lenght is 50 and min is 5")]
        public string? Name { get; set; }
        public decimal Salary { get; set; }


        [Range(22, 30)]
        public int Age { get; set; }


        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}$"
                , ErrorMessage = "addres must be like 123-street-city-country")]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public IFormFile Image { get; set; } //هناك model  فى ال  mapping مش هيحصلها   

        public string ImageName { get; set; }    // mappingدا ال هيحصله 

        //navegatin property [one]
        public Department Department { get; set; }

        //[ForeignKey("Department")]
        public int? DepartmentId { get; set; } // forgine key

    }
}
