using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models.Operator;

namespace Server.Models
{
	public class AppContext : IdentityDbContext<Account>
	{
		public AppContext(DbContextOptions<AppContext> options) : base(options) {
			Database.EnsureCreated();
		}
	}
}
