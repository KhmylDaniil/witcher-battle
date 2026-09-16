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
			// NpgsqlDataSource — это пул подключений, а не просто конфигурация; Build() рассчитан на однократный
			// вызов. AddDbContext резолвит DbContextOptions<TDbContext> в каждом новом scope (на каждый запрос),
			// поэтому дата-сорс собирается здесь один раз заранее и переиспользуется, а не пересобирается
			// в options-делегате на каждый resolve.
			var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString(connectionStringName))
				.EnableDynamicJson()
				.ConfigureJsonOptions(new JsonSerializerOptions { AllowOutOfOrderMetadataProperties = true })
				.Build();

			services.AddDbContext<DbContext, TDbContext>(options => options.UseNpgsql(dataSource));

			return services;
		}
	}
}
