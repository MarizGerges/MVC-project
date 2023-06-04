using System.ComponentModel.DataAnnotations;

namespace session3Mvc.ViewModels
{
	public class RegisterViewModel
	{

		public string FName { get; set; }
		public string LName { get; set; }

		[Required(ErrorMessage ="email is required")]
		[EmailAddress(ErrorMessage ="invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "conferm password is required")]
		[Compare("Password",ErrorMessage ="confirm password does not match password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }


	}
}
