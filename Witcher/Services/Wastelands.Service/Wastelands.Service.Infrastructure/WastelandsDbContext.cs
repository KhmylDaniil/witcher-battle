using Microsoft.EntityFrameworkCore;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure
{
    public class WastelandsDbContext : DbContext
    {
		DbSet<User> Users { get; set; }

		DbSet<Character> Characters { get; set; }

		public WastelandsDbContext(DbContextOptions<WastelandsDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(WastelandsDbContext).Assembly);
		}
	}
}
