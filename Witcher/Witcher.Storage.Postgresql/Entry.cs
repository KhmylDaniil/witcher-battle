using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Witcher.Core.Abstractions;

namespace Witcher.Storage.MySql
{
	public static class Entry
	{
		public static IServiceCollection AddMySqlStorage
			(this IServiceCollection services,
			IConfiguration configuration,
			Microsoft.Extensions.Logging.ILoggerFactory sqlLoggerFactory)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			var connectionString = configuration.GetConnectionString("mySql");
			var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

			services.AddDbContext<AppDbContext>(opt => opt.UseMySql(connectionString, serverVersion,
				options => options.SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore))
				.UseLoggerFactory(sqlLoggerFactory));
			services.AddTransient<IAppDbContext, AppDbContext>();

			return services;
		}

		public static void MigrateDB(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetService<AppDbContext>()
				?? throw new Exception("This should never happen, the DbContext couldn't recolve!");

			dbContext.Database.Migrate();
		}
	}
}
