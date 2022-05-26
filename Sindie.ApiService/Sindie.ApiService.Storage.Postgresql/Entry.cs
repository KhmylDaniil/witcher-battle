using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Storage.Postgresql
{
	/// <summary>
	/// точка входа
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить зависимости проекта PostgreSql
		/// </summary>
		/// <param name="services">Сервисы IServiceCollection</param>
		/// <param name="options">Параметры PostgreSqlOptions</param>
		/// <returns>Сервисы</returns>
		public static IServiceCollection AddPostgreSqlStorage
			(this IServiceCollection services,
			PostgreSqlOptions options,
			Microsoft.Extensions.Logging.ILoggerFactory sqlLoggerFactory)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));
			if (string.IsNullOrWhiteSpace(options?.ConnectionString))
				throw new ArgumentNullException(nameof(options));

			services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(options.ConnectionString)
				.UseLoggerFactory(sqlLoggerFactory));
			services.AddTransient<IAppDbContext, AppDbContext>();

			return services;
		}
	}
}
