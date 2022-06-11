using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
	/// <summary>
	/// Model for change role page.
	/// </summary>
	public class ChangeRoleModel
	{
		/// <summary>
		/// User id.
		/// </summary>
		public string UserId { get; set; }
		
		/// <summary>
		/// User email.
		/// </summary>
		public string UserEmail { get; set; }

		/// <summary>
		/// All active roles.
		/// </summary>
		public List<IdentityRole> AllRoles { get; set; }
		
		/// <summary>
		/// New user roles.
		/// </summary>
		public IList<string> UserRoles { get; set; }

		/// <summary>
		/// Model for change role page.
		/// </summary>
		public ChangeRoleModel() {
			AllRoles = new List<IdentityRole>();
			UserRoles = new List<string>();
		}
	}
}
