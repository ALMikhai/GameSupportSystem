using DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models.Operator;

namespace Server.Models
{
	/// <summary>
	/// Application database context.
	/// </summary>
	public class AppContext : IdentityDbContext<Account>
	{
		/// <summary>
		/// Players database.
		/// </summary>
		public DbSet<Player> Players { get; set; } = null!;
		
		/// <summary>
		/// Messages database.
		/// </summary>
		public DbSet<Message> Messages { get; set; } = null!;
		
		/// <summary>
		/// Player tokens database.
		/// </summary>
		public DbSet<PlayerToken> Tokens { get; set; } = null!;

		/// <summary>
		/// Application database context.
		/// </summary>
		/// <param name="options">Context options.</param>
		public AppContext(DbContextOptions<AppContext> options) : base(options) {
			Database.EnsureCreated();
		}
	}
}
