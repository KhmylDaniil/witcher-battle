using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wastelands.EfDataAccess.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddServiceDbContext<TDbContext>(
			this IServiceCollection services,
			IConfiguration configuration,
			string connectionStringName)
			where TDbContext : DbContext
		{
			services.AddDbContext<DbContext, TDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(connectionStringName)));

			return services;
		}
	}
}
