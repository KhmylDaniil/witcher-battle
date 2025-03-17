using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wastelands.EfDataAccess.Extensions
{
	public static class WebApplicationExtensions
	{
		public static async Task<IApplicationBuilder> MigrateDatabaseAsync<TDbContext>(this IApplicationBuilder app)
			where TDbContext : DbContext
		{
			await using var scope = app.ApplicationServices.CreateAsyncScope();
			await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
			await context.Database.MigrateAsync();

			return app;
		}
	}
}
