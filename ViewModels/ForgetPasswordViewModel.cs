using System.ComponentModel.DataAnnotations;

namespace session3Mvc.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage = "email is required")]
		[EmailAddress(ErrorMessage = "invalid Email")]
		public string Email { get; set; }
	}
}
