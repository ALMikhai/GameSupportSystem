using System.ComponentModel.DataAnnotations;

namespace Server.Models.Operator
{
	/// <summary>
	/// Model for login page.
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		/// Email.
		/// </summary>
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		/// Password string.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Flag to remember the user.
		/// </summary>
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
