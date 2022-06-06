using DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models.Operator;

namespace Server.Models
{
	public class AppContext : IdentityDbContext<Account>
	{
		public DbSet<Player> Players { get; set; } = null!;
		public DbSet<Chat> Chats { get; set; } = null!;
		public DbSet<PlayerToken> Tokens { get; set; } = null!;

		public AppContext(DbContextOptions<AppContext> options) : base(options) {
			Database.EnsureCreated();
		}
	}
}
