using Microsoft.Extensions.DependencyInjection;

namespace Sindie.ApiService.WebApi.Versioning
{
	/// <summary>
	/// Точка входа сервисов версионирования для приложения
	/// </summary>
	public static class Entry
	{
		/// <summary>
		/// Добавить версионирование API
		/// </summary>
		/// <param name="services">Коллекция служб проекта</param>
		/// <returns>Службы с версионированием</returns>
		public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services) =>
			services
				.AddApiVersioning(
					options =>
					{
						options.AssumeDefaultVersionWhenUnspecified = true;
						options.ReportApiVersions = true;
					})
				// Формат версии: 'v'major[.minor][-status]
				.AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'VVV");
	}
}
