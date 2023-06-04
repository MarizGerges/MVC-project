using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace session3Mvc.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "name is requierd")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "code is requierd")]
        public string Code { get; set; }
        //navegational property [many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
