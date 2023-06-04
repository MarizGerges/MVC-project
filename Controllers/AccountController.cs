using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using session3Mvc.Helpers;
using session3Mvc.ViewModels;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace session3Mvc.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager1;

		public AccountController( UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager1)
        {
			_userManager = userManager;
			_signInManager1 = signInManager1;
		}
        #region  Register

        public IActionResult Register ()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = new ApplicationUser()
				{
					FName = model.FName,
					LName = model.LName,
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					IsAgree = model.IsAgree,
				};

				var result =await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login)); 
				}
				foreach(var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description); 
				}

			}
			return View(model);
		}


		#endregion

		#region login

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user =await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						await _signInManager1.PasswordSignInAsync(user,model.Password,model.RememberMe, false);
						return RedirectToAction("Index","Home");

					}
					ModelState.AddModelError(string.Empty, " invalid password");
				}
				ModelState.AddModelError(string.Empty, "email is not exist");
			}
			return View(model);
		}

		#endregion
		#region sign out
		public new async Task<IActionResult> SignOut()
		{
			await  _signInManager1.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task< IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user =await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token=await _userManager.GeneratePasswordResetTokenAsync(user); // token valid for this user only one-time
					var passwordResetLink=Url.Action("ResetPassword", "Account", new {email=user.Email , token = token } , Request.Scheme ); // https://localhost:44325/Account/ResetPassword?email=mariz@gmail.com&token=15ndlknjnsdbj

					var Email = new Email()
					{
						Subject = "Reset Password",
						To = model.Email,
						Body = passwordResetLink  // "this is reset Password link"
					};
					EmailSetting.SendEmail(Email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "email is not existed");

			}
			return View(model);
		}
		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion

		#region resetPassword
		public IActionResult ResetPassword(string email , string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}

		#endregion
	}
}
