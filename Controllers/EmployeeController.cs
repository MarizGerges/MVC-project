using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Reposatries;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using session3Mvc.Helpers;
using session3Mvc.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session3Mvc.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmpeloyeeReposatry _empeloyeeReposatry;
        //private readonly IDepartmentReposatry _departmentReposatry;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper) // asking clr to create object from IDepartmentReposatry and assign his adr
        {
            //_empeloyeeReposatry = empeloyeeReposatry;
            //_departmentReposatry = departmentReposatry;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task< IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                 employees =await _unitOfWork.EmpeloyeeReposatry.GetAll();
            }
            else
            {
                 employees= _unitOfWork.EmpeloyeeReposatry.SearchEmployeesByName(SearchValue);

            }
            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmps);

        }
        //[HttpGet]
        public  async Task <IActionResult> Create()
        {
            ViewBag.Departments=await _unitOfWork.DepartmentReposatry.GetAll();
            return View();

        }

        [HttpPost]
        public async Task< IActionResult> Create(EmployeeViewModel employeeVM)
        {
            ///manual Mapping::
            ///var employee = new Employee()
            ///{
            ///    Name = employeeVM.Name,
            ///   Address = employeeVM.Address,
            ///    Email = employeeVM.Email,
            ///    Salary = employeeVM.Salary,
            ///    Age = employeeVM.Age,
            ///    DepartmentId = employeeVM.DepartmentId,
            ///    IsActive = employeeVM.IsActive,
            ///    HireDate = employeeVM.HireDate,
            ///   PhoneNumber = employeeVM.PhoneNumber
            ///};
             
            employeeVM.ImageName = DoucmentSettings.UploadFile(employeeVM.Image, "images");
            var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            if (ModelState.IsValid)
            {

                _unitOfWork.EmpeloyeeReposatry.Add(mappedEmp);
                int count =await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee created succesfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mappedEmp);
        }

        //[HttpGet]
        public async Task< IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee =await _unitOfWork.EmpeloyeeReposatry.GetById(id.Value);
            if (employee is null)
            {
                return NotFound();
            }

            var mappedEmp=_mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, mappedEmp);

        }
        public async Task  <IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var department = _departmentRepositry.GetById(id.Value);
            //if (department is null)
            //{
            //    return NotFound();
            //}
            //return View(department);
            ViewBag.Departments = await _unitOfWork.DepartmentReposatry.GetAll();
            return await Details(id, "Edit");


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    employeeVM.ImageName = DoucmentSettings.UploadFile(employeeVM.Image, "images");
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmpeloyeeReposatry.Update(mappedEmp);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    //1.log exception
                    //2. frindly message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);

        }



        [HttpGet]
        public  async Task< IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");

        }
        [HttpPost]
        public async Task< IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                 _unitOfWork.EmpeloyeeReposatry.Delete(mappedEmp);
                int count =await _unitOfWork.Complete();
                if (count > 0)
                    DoucmentSettings.DeleteFile(employeeVM.ImageName, "images");
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //1.log exception
                //2. frindly message

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
    }

}

