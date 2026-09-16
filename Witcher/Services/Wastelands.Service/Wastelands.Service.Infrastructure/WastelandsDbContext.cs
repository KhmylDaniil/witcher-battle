using Microsoft.EntityFrameworkCore;

namespace Wastelands.Service.Infrastructure
{
    public class WastelandsDbContext : DbContext
    {
		// Намеренно без DbSet<T>-свойств: EF узнаёт о сущностях через IEntityTypeConfiguration<T>
		// (см. ApplyConfigurationsFromAssembly ниже), а все запросы идут через generic _context.Set<T>()
		// в BaseReadRepository — свойства DbSet здесь были бы мёртвым кодом.

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
