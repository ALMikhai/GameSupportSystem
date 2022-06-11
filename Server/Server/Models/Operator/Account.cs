using Microsoft.AspNetCore.Identity;

namespace Server.Models.Operator
{
	/// <summary>
	/// Operator account.
	/// </summary>
	public class Account : IdentityUser
	{
		/// <summary>
		/// Operator name.
		/// </summary>
		public string Name { get; set; }
	}
}
