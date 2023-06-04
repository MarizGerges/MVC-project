using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Reposatries;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using session3Mvc.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session3Mvc.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) // asking clr to create object from IDepartmentReposatry and assign his adr
        { 
        
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task< IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentReposatry.GetAll();
            var mappeDeps = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(mappeDeps);
        }
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        
        }
        [HttpPost]
        public async Task<IActionResult> Create( DepartmentViewModel departmentMV)
        {
            var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentMV);

            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentReposatry.Add(mappedDept);
                int count =await _unitOfWork.Complete();
                if (count>0)
                      TempData["Message"] = "Department is created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(mappedDept);
        }

        //[HttpGet]
        public async Task<IActionResult> Details(int? id , string ViewName="Details")
        {
            if(id is null)
                return BadRequest();
            var department = await _unitOfWork.DepartmentReposatry.GetById(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(ViewName, mappedDept);

        }
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var department = _departmentRepositry.GetById(id.Value);
            //if (department is null)
            //{
            //    return NotFound();
            //}
            //return View(department);

            return await Details(id, "Edit");


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentMV) 
        {
            if (id != departmentMV.Id)
                return BadRequest();
            var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentMV);

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentReposatry.Update(mappedDept);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex) 
                {
                   //1.log exception
                   //2. frindly message

                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            }
            return View(mappedDept);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");

        }
        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentMV)
        {
                try
                {
                var mappedDept = _mapper.Map<DepartmentViewModel,Department>(departmentMV);

                _unitOfWork.DepartmentReposatry.Delete(mappedDept);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    //1.log exception
                    //2. frindly message

                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(departmentMV);
                }
            }
          

        }

}

