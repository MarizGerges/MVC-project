using AutoMapper;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using session3Mvc.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace session3Mvc.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel , IdentityRole>(model);

                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName=R.Name
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                var mappedRole = new RoleViewModel()
                {
                    Id = role.Id,
                    RoleName=role.Name
                };
                return View(new List<RoleViewModel>() { mappedRole });
            }
        }

        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(ViewName, mappedUser);

        }

        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (id != updatedRole.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);
                    Role.Name = updatedRole.RoleName;
                   
                    //user.Email = updatedUser.Email;
                    //user.SecurityStamp=Guid.NewGuid().ToString();

                    await _roleManager.UpdateAsync(Role);

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    //1.log exception
                    //2. frindly message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(updatedRole);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");

        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //1.log exception
                //2. frindly message

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
