using System.ComponentModel.DataAnnotations;

namespace session3Mvc.ViewModels
{
	public class LoginViewModel
	{
		

		[Required(ErrorMessage = "email is required")]
		[EmailAddress(ErrorMessage = "invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		public bool RememberMe { get; set; }
	}
}
