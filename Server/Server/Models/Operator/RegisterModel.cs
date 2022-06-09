using System.ComponentModel.DataAnnotations;

namespace Server.Models.Operator
{
	public class RegisterModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		public string PasswordConfirm { get; set; }
	}
}
