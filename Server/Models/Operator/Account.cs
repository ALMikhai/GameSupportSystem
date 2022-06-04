using Microsoft.AspNetCore.Identity;

namespace Server.Models.Operator
{
	public class Account : IdentityUser
	{
		public string Name { get; set; }
	}
}
