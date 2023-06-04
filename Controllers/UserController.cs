using AutoMapper;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using session3Mvc.Helpers;
using session3Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace session3Mvc.Controllers
{
	public class UserController: Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager ,IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }

		public async Task< IActionResult> Index(string email)
		{
			if(string.IsNullOrEmpty(email))
			{
				var users =await _userManager.Users.Select(u=> new UserViewModel()
				{
					Id = u.Id,
					FName=u.FName,
					LName=u.LName,
					Email=u.Email,
					PhoneNumber=u.PhoneNumber,
					Roles= _userManager.GetRolesAsync(u).Result 
				}).ToListAsync();
				return View(users);
			}
			else
			{
				var user =await _userManager.FindByEmailAsync(email);
				var mappedUser = new UserViewModel()
				{
					Id = user.Id ,
					FName = user.FName,
					LName = user.LName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				return View(new List<UserViewModel>() {mappedUser });
			}
		}

        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

			var mappedUser = _mapper.Map < ApplicationUser, UserViewModel>(user);
            return View(ViewName, mappedUser);

        }

        public async Task<IActionResult> Edit(string? id)
        {
            
            return await Details(id, "Edit");


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit([FromRoute] string id, UserViewModel updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    user.FName = updatedUser.FName;
                    user.LName = updatedUser.LName;
                    user.PhoneNumber = updatedUser.PhoneNumber;
                    //user.Email = updatedUser.Email;
                    //user.SecurityStamp=Guid.NewGuid().ToString();

                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    //1.log exception
                    //2. frindly message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(updatedUser);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");

        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete( string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                
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
