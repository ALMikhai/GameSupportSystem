using System.ComponentModel.DataAnnotations;

namespace Server.Models.Operator
{
	/// <summary>
	/// Model for register page.
	/// </summary>
	public class RegisterModel
	{
		/// <summary>
		/// Email.
		/// </summary>
		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		/// Operator name.
		/// </summary>
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		/// <summary>
		/// Password string.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Confirm password string.
		/// </summary>
		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		public string PasswordConfirm { get; set; }
	}
}
