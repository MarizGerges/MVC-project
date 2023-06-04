using System.ComponentModel.DataAnnotations;

namespace session3Mvc.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "password is required")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "conferm password is required")]
		[Compare("NewPassword", ErrorMessage = "confirm password does not match password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

	}
}
