using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Text.Json;

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
			var builder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString(connectionStringName));
			builder.EnableDynamicJson().ConfigureJsonOptions(new JsonSerializerOptions { AllowOutOfOrderMetadataProperties = true });

			services.AddDbContext<DbContext, TDbContext>(options => options.UseNpgsql(builder.Build()));

			return services;
		}
	}
}
